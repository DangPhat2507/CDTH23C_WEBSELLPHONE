using Microsoft.AspNetCore.Mvc;
using web_SellPhone.Models;
using Microsoft.EntityFrameworkCore;

namespace web_SellPhone.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        // Trang ch?
        public IActionResult Index()
        {
            var sp = _context.Products
                             .Include(p => p.Category)
                             .Take(12)  // l?y 12 s?n ph?m ð?u tiên
                             .ToList();
            return View(sp);
        }
    }
}
