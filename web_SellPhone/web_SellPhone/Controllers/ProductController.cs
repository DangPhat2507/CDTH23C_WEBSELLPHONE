using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_SellPhone.Models;

namespace web_SellPhone.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext _context;

        public ProductController(MyDbContext context)
        {
            _context = context;
        }

        // Trang chi tiết sản phẩm
        public IActionResult Detail(int id)
        {
            var sp = _context.Products
                             .Include(p => p.Category)
                             .FirstOrDefault(p => p.MaSP == id);

            if (sp == null)
            {
                return NotFound();
            }

            return View(sp);
        }
    }
}
