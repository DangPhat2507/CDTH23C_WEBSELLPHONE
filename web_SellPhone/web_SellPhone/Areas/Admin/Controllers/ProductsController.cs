using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_SellPhone.Models;

namespace web_SellPhone_NEW.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly MyDbContext _context;

        public ProductsController(MyDbContext context)
        {
            _context = context;
        }

        // Danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var list = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
            return View(list);
        }

        // Chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var sp = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.MaSP == id);

            if (sp == null) return NotFound();
            return View(sp);
        }

        // Tạo sản phẩm
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(model);
        }

        // Chỉnh sửa
        public async Task<IActionResult> Edit(int id)
        {
            var sp = await _context.Products.FindAsync(id);
            if (sp == null) return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            if (id != model.MaSP) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(model);
        }

        // Xóa
        public async Task<IActionResult> Delete(int id)
        {
            var sp = await _context.Products.FindAsync(id);
            if (sp == null) return NotFound();
            return View(sp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sp = await _context.Products.FindAsync(id);
            if (sp != null)
            {
                _context.Products.Remove(sp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
