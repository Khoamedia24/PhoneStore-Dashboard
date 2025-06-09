using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Customer.Models;
using PhoneStore.Customer.ViewModels;

namespace PhoneStore.Customer.Controllers
{
    public class ProductController : Controller
    {
        private readonly PhoneStoreContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(PhoneStoreContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }        // GET: Product
        public async Task<IActionResult> Index(
            string? search,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string sortBy = "name",
            int page = 1,
            int pageSize = 12)
        {
            var productsQuery = _context.Products
                .Where(p => p.IsPublished)
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .Include(p => p.ProductImages)
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                productsQuery = productsQuery.Where(p =>
                    p.ProductName.Contains(search) ||
                    (p.ShortDescription != null && p.ShortDescription.Contains(search)));
            }

            // Apply category filter
            if (categoryId.HasValue && categoryId > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
            }

            // Apply custom price range
            if (minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice);
            }

            // Apply sorting
            productsQuery = sortBy.ToLower() switch
            {
                "price_asc" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                "newest" => productsQuery.OrderByDescending(p => p.ProductId),
                "oldest" => productsQuery.OrderBy(p => p.ProductId),
                _ => productsQuery.OrderBy(p => p.ProductName)
            };            var totalProducts = await productsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            var products = await productsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productViewModels = products.Select(p => new ProductCardViewModel
            {
                Id = p.ProductId,
                ProductId = p.ProductId,
                Name = p.ProductName ?? "",
                Brand = "", // No brand field in simplified model
                Description = p.ShortDescription,
                Price = p.Price,
                DiscountPrice = p.Discount?.DiscountPercent != null ?
                    p.Price * (1 - (decimal)p.Discount.DiscountPercent.Value / 100) : null,
                PrimaryImageUrl = p.ProductImages.Any() ?
                    p.ProductImages.First().ImageUrl : "/images/no-image.png",
                CategoryName = p.Category?.CategoryName ?? "",
                ColorName = "" // Simplified color handling
            }).ToList();

            var viewModel = new ProductListViewModel
            {
                Products = productViewModels,
                Categories = await _context.Categories.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                CategoryId = categoryId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                Search = search ?? "",
                CurrentPage = page,
                TotalPages = totalPages,
                TotalProducts = totalProducts,
                PageSize = pageSize
            };

            return View(viewModel);
        }        // GET: Product/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsPublished);

            if (product == null)
            {
                return NotFound();
            }

            // Get related products from the same category
            var relatedProducts = await _context.Products
                .Where(p => p.IsPublished && p.CategoryId == product.CategoryId && p.ProductId != id)
                .Include(p => p.ProductImages)
                .Include(p => p.Discount)
                .Take(4)
                .ToListAsync();

            var viewModel = new ProductDetailViewModel
            {
                Id = product.ProductId,
                Name = product.ProductName ?? "",
                Description = product.DetailDescription ?? product.ShortDescription,
                Price = product.Discount?.DiscountPercent != null ?
                    product.Price * (1 - (decimal)product.Discount.DiscountPercent.Value / 100) : product.Price,
                OriginalPrice = product.Price,
                StockQuantity = product.Stock,
                CategoryName = product.Category?.CategoryName ?? "",
                Images = product.ProductImages.Select(pi => new ProductImageViewModel
                {
                    ImageUrl = pi.ImageUrl,
                    AltText = product.ProductName ?? ""
                }).ToList(),
                RelatedProducts = relatedProducts.Select(rp => new ProductCardViewModel
                {
                    Id = rp.ProductId,
                    ProductId = rp.ProductId,
                    Name = rp.ProductName ?? "",
                    Brand = "", // No brand field
                    Price = rp.Discount?.DiscountPercent != null ?
                        rp.Price * (1 - (decimal)rp.Discount.DiscountPercent.Value / 100) : rp.Price,
                    OriginalPrice = rp.Price,
                    ImageUrl = rp.ProductImages.FirstOrDefault()?.ImageUrl ?? "/images/no-image.png"
                }).ToList()
            };

            return View(viewModel);
        }        // GET: Product/Search - For AJAX search
        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(new List<object>());
            }

            var products = await _context.Products
                .Where(p => p.IsPublished &&
                    (p.ProductName.Contains(term) ||
                     (p.ShortDescription != null && p.ShortDescription.Contains(term))))
                .Include(p => p.ProductImages)
                .Take(10)
                .Select(p => new
                {
                    id = p.ProductId,
                    name = p.ProductName,
                    price = p.Price,
                    imageUrl = p.ProductImages.Any() ?
                        p.ProductImages.First().ImageUrl : "/images/no-image.png"
                })
                .ToListAsync();

            return Json(products);
        }

        // GET: Product/QuickView/5 - For quick view modal data
        [HttpGet]
        public async Task<IActionResult> QuickView(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Discount)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsPublished);

            if (product == null)
            {
                return NotFound();
            }            var quickViewData = new
            {
                id = product.ProductId,
                name = product.ProductName ?? "",
                description = product.ShortDescription ?? "",
                price = product.Price,
                discountPrice = product.Discount?.DiscountPercent != null ?
                    product.Price * (1 - (decimal)product.Discount.DiscountPercent.Value / 100) : (decimal?)null,
                originalPrice = product.Price,
                stockQuantity = product.Stock,
                categoryName = product.Category?.CategoryName ?? "",
                images = product.ProductImages.Select(pi => pi.ImageUrl).ToList()
            };

            return Json(quickViewData);
        }

        // GET: Product/GetImage/5 - For serving product images
        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image?.ImageData == null)
            {
                return NotFound();
            }

            return File(image.ImageData, image.ImageMimeType ?? "image/jpeg");
        }
    }
}
