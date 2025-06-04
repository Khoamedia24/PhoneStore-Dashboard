using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PhoneStore.Models;
using PhoneStore.Services;
using PhoneStore.Attributes;

namespace PhoneStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly PhoneStoreContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductImageService _imageService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            PhoneStoreContext context,
            IWebHostEnvironment webHostEnvironment,
            IProductImageService imageService,            ILogger<ProductController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AdminAuthorize]
        public async Task<IActionResult> Index(string searchTerm, string category, string color, string sortBy, int page = 1)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                    .ThenInclude(pi => pi.Color)
                .Include(p => p.Discount)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => 
                    (p.ProductName != null && p.ProductName.Contains(searchTerm)) || 
                    (p.DetailDescription != null && p.DetailDescription.Contains(searchTerm)));
            }

            // Lọc theo danh mục
            if (!string.IsNullOrEmpty(category) && int.TryParse(category, out int categoryId))
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            // Lọc theo màu sắc
            if (!string.IsNullOrEmpty(color) && int.TryParse(color, out int colorId))
            {
                query = query.Where(p => p.ProductImages.Any(pi => pi.ColorId == colorId));
            }

            // Sắp xếp
            query = sortBy switch
            {
                "name" => query.OrderBy(p => p.ProductName),
                "price" => query.OrderBy(p => p.Price),
                "stock" => query.OrderBy(p => p.Stock),
                _ => query.OrderByDescending(p => p.ProductId)
            };

            // Phân trang
            int pageSize = 12;
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Lấy danh sách danh mục cho dropdown
            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem 
                { 
                    Value = c.CategoryId.ToString(), 
                    Text = c.CategoryName ?? string.Empty
                })
                .ToListAsync();

            // Lấy danh sách màu sắc cho dropdown
            ViewBag.Colors = await _context.Colors
                .Select(c => new SelectListItem 
                { 
                    Value = c.ColorId.ToString(), 
                    Text = c.ColorName ?? string.Empty
                })
                .ToListAsync();

            return View(products);
        }

        [AdminAuthorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName");
            ViewBag.Colors = new SelectList(await _context.Colors.ToListAsync(), "ColorId", "ColorName");
            ViewBag.Discounts = new SelectList(await _context.DiscountPrograms.ToListAsync(), "DiscountId", "DiscountName");
            return View();
        }        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)] // 100MB total limit
        public async Task<IActionResult> Create(Product product)
        {
            _logger.LogInformation("=== CREATE PRODUCT REQUEST STARTED ===");
            _logger.LogInformation("Product data received: {@Product}", product);
            
            // Extract colors and images from form
            var colors = new List<int>();
            var imagesByColor = new List<List<IFormFile>>();
            
            // Log form data
            foreach (var key in Request.Form.Keys)
            {
                _logger.LogInformation("Form key: {Key}, Value: {Value}", key, Request.Form[key]);
                
                if (key == "Colors")
                {
                    foreach (var colorValue in Request.Form[key])
                    {
                        if (int.TryParse(colorValue, out int colorId))
                        {
                            colors.Add(colorId);
                        }
                    }
                }
            }

            // Group files by color sections
            var colorIndex = 0;
            var filesBySection = new Dictionary<int, List<IFormFile>>();
            
            foreach (var file in Request.Form.Files)
            {
                _logger.LogInformation("File: {FileName}, Size: {Size}, ContentType: {ContentType}", 
                    file.FileName, file.Length, file.ContentType);
                    
                if (file.Name == "Images")
                {
                    if (!filesBySection.ContainsKey(colorIndex))
                    {
                        filesBySection[colorIndex] = new List<IFormFile>();
                    }
                    filesBySection[colorIndex].Add(file);
                }
            }
            
            // Convert to expected format
            for (int i = 0; i < colors.Count; i++)
            {
                if (filesBySection.ContainsKey(i))
                {
                    imagesByColor.Add(filesBySection[i]);
                }
                else
                {
                    imagesByColor.Add(new List<IFormFile>());
                }
            }
              _logger.LogInformation("Colors processed: {Colors}", colors.Count > 0 ? string.Join(", ", colors) : "None");
            _logger.LogInformation("Image groups processed: {ImageCount}", imagesByColor.Count);
            
            async Task<IActionResult> PrepareViewBagsAndReturn()
            {
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                ViewBag.Colors = new SelectList(await _context.Colors.ToListAsync(), "ColorId", "ColorName");
                ViewBag.Discounts = new SelectList(await _context.DiscountPrograms.ToListAsync(), "DiscountId", "DiscountName", product.DiscountId);
                return View(product);
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                _logger.LogWarning("Model validation failed: {Errors}", string.Join(", ", errors));
                return await PrepareViewBagsAndReturn();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (colors == null || colors.Count == 0)
                {
                    _logger.LogWarning("No colors selected");
                    ModelState.AddModelError("", "Vui lòng chọn ít nhất một màu sắc");
                    return await PrepareViewBagsAndReturn();
                }

                if (imagesByColor == null || imagesByColor.Count != colors.Count)
                {
                    _logger.LogWarning("Image groups count ({ImageCount}) does not match colors count ({ColorCount})",
                        imagesByColor?.Count ?? 0, colors.Count);
                    ModelState.AddModelError("", "Số lượng màu sắc và nhóm ảnh không khớp");
                    return await PrepareViewBagsAndReturn();
                }

                // First save the product
                _context.Products.Add(product);
                var saveResult = await _context.SaveChangesAsync();
                _logger.LogInformation("Created product with ID: {ProductId}, SaveChanges result: {SaveResult}", 
                    product.ProductId, saveResult);

                if (saveResult <= 0)
                {
                    _logger.LogError("Failed to save product to database");
                    ModelState.AddModelError("", "Không thể lưu thông tin sản phẩm");
                    return await PrepareViewBagsAndReturn();
                }

                // Process images for each color
                for (int i = 0; i < colors.Count; i++)
                {
                    var colorId = colors[i];
                    var colorImages = imagesByColor[i];

                    _logger.LogInformation("Processing images for color {ColorId}, image count: {ImageCount}",
                        colorId, colorImages?.Count ?? 0);

                    if (colorImages == null || !colorImages.Any())
                    {
                        _logger.LogWarning("No images provided for color {ColorId}", colorId);
                        ModelState.AddModelError("", $"Vui lòng chọn ít nhất một ảnh cho màu thứ {i + 1}");
                        await transaction.RollbackAsync();
                        return await PrepareViewBagsAndReturn();
                    }

                    foreach (var image in colorImages.Where(i => i != null && i.Length > 0))
                    {
                        try
                        {
                            _logger.LogInformation("Processing image {FileName} ({Length} bytes) for color {ColorId}",
                                image.FileName, image.Length, colorId);

                            var imageData = await _imageService.ProcessImageAsync(image);
                            if (imageData == null)
                            {
                                _logger.LogWarning("Image processing returned null for {FileName}", image.FileName);
                                continue;
                            }

                            var productImage = new ProductImage
                            {
                                ProductId = product.ProductId,
                                ColorId = colorId,
                                ImageData = imageData,
                                ImageMimeType = "image/jpeg"
                            };

                            _context.ProductImages.Add(productImage);
                            var imageSaveResult = await _context.SaveChangesAsync();
                            
                            _logger.LogInformation(
                                "Saved image for product {ProductId}, color {ColorId}, size {Size} bytes, SaveChanges result: {SaveResult}",
                                product.ProductId,
                                colorId,
                                imageData.Length,
                                imageSaveResult);

                            if (imageSaveResult <= 0)
                            {
                                _logger.LogError("Failed to save image to database");
                                ModelState.AddModelError("", "Không thể lưu hình ảnh sản phẩm");
                                await transaction.RollbackAsync();
                                return await PrepareViewBagsAndReturn();
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            _logger.LogError(ex, "Validation error processing image for product {ProductId}", product.ProductId);
                            ModelState.AddModelError("", ex.Message);
                            await transaction.RollbackAsync();
                            return await PrepareViewBagsAndReturn();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing image for product {ProductId}", product.ProductId);
                            ModelState.AddModelError("", $"Lỗi xử lý ảnh: {ex.Message}");
                            await transaction.RollbackAsync();
                            return await PrepareViewBagsAndReturn();
                        }
                    }
                }

                await transaction.CommitAsync();
                _logger.LogInformation("Successfully completed product creation for {ProductId}", product.ProductId);
                TempData["Success"] = "Thêm sản phẩm thành công";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product: {Message}", ex.Message);
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm sản phẩm: " + ex.Message);
                return await PrepareViewBagsAndReturn();
            }
        }

        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm" });
                }

                _context.ProductImages.RemoveRange(product.ProductImages);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                
                return Json(new { success = true, message = "Xóa sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa sản phẩm: " + ex.Message });
            }
        }

        [AdminAuthorize]
        public async Task<IActionResult> GetColorImages(int productId, int colorId)
        {
            var images = await _context.ProductImages
                .Where(pi => pi.ProductId == productId && pi.ColorId == colorId)
                .Select(pi => new { 
                    imageId = pi.ImageId,
                    imageUrl = $"/Product/GetImage/{pi.ImageId}"
                })
                .ToListAsync();

            return Json(new { success = true, images = images });
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image == null || image.ImageData == null || string.IsNullOrEmpty(image.ImageMimeType))
            {
                _logger.LogWarning("Image not found or invalid for ID: {ImageId}", id);
                return NotFound();
            }

            _logger.LogInformation("Serving image ID: {ImageId}, Size: {Size} bytes, Type: {MimeType}",
                id, image.ImageData.Length, image.ImageMimeType);
            return File(image.ImageData, image.ImageMimeType);
        }

        [AdminAuthorize]
        [HttpGet]
        public async Task<IActionResult> CreateTest()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName");
            ViewBag.Colors = new SelectList(await _context.Colors.ToListAsync(), "ColorId", "ColorName");
            ViewBag.Discounts = new SelectList(await _context.DiscountPrograms.ToListAsync(), "DiscountId", "DiscountName");
            return View();
        }

        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTest(Product product)
        {
            _logger.LogInformation("=== CREATE TEST PRODUCT ===");
            _logger.LogInformation("Product: {@Product}", product);
            _logger.LogInformation("ModelState.IsValid: {IsValid}", ModelState.IsValid);
            
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    _logger.LogWarning("ModelState Error - Key: {Key}, Errors: {Errors}", 
                        error.Key, string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage)));
                }
                
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                ViewBag.Colors = new SelectList(await _context.Colors.ToListAsync(), "ColorId", "ColorName");
                ViewBag.Discounts = new SelectList(await _context.DiscountPrograms.ToListAsync(), "DiscountId", "DiscountName", product.DiscountId);
                return View(product);
            }

            try
            {
                _context.Products.Add(product);
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("SaveChanges result: {Result}, Product ID: {ProductId}", result, product.ProductId);
                
                TempData["Success"] = "Test product created successfully without images!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating test product");
                ModelState.AddModelError("", "Error: " + ex.Message);
                
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                ViewBag.Colors = new SelectList(await _context.Colors.ToListAsync(), "ColorId", "ColorName");
                ViewBag.Discounts = new SelectList(await _context.DiscountPrograms.ToListAsync(), "DiscountId", "DiscountName", product.DiscountId);
                return View(product);
            }
        }
    }
}
