using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;
using PhoneStore.Attributes;

namespace PhoneStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly PhoneStoreContext _context;

        public CategoryController(PhoneStoreContext context)
        {
            _context = context;        }

        [AdminAuthorize(area: "Category", action: "Index")]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
            return View(categories);        }

        [AdminAuthorize(area: "Category", action: "Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });        }

        [AdminAuthorize(area: "Category", action: "Edit")]
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _context.Categories.FindAsync(category.CategoryId);
                    if (existingCategory == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy danh mục" });
                    }

                    existingCategory.CategoryName = category.CategoryName;
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ" });        }

        [AdminAuthorize(area: "Category", action: "Delete")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return Json(new { success = false, message = "Không tìm thấy danh mục" });
            }

            if (category.Products.Any())
            {
                return Json(new { success = false, message = "Không thể xóa danh mục đang được sử dụng trong sản phẩm" });
            }

            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
