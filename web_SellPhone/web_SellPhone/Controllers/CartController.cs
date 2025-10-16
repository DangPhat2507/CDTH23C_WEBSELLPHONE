using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using web_SellPhone.Models;
using Microsoft.AspNetCore.Http;

namespace web_SellPhone.Controllers
{
    public class CartController : Controller
    {
        private readonly MyDbContext _context;
        private const string CART_KEY = "GioHang";

        public CartController(MyDbContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString(CART_KEY);
            if (cart == null)
            {
                return new List<CartItem>();
            }
            return JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        // Lưu giỏ hàng vào Session
        private void SaveCart(List<CartItem> cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(CART_KEY, json);
        }

        // Trang xem giỏ hàng
        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.Total = cart.Sum(x => x.ThanhTien);
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ
        public IActionResult Add(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);
            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaSP = product.MaSP,
                    TenSP = product.TenSP,
                    DonGia = product.DonGiaKhuyenMai ?? product.DonGia,
                    HinhAnh = product.HinhAnh,
                    SoLuong = 1
                });
            }
            else
            {
                item.SoLuong++;
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ
        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaSP == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }

        // Xóa toàn bộ giỏ
        public IActionResult Clear()
        {
            SaveCart(new List<CartItem>());
            return RedirectToAction("Index");
        }
    }
}
