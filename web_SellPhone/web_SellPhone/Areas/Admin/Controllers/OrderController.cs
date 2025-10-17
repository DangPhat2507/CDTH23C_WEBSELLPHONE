using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using web_SellPhone.Models;

namespace web_SellPhone.Controllers
{
    [Authorize] // bắt buộc đăng nhập mới đặt hàng
    public class OrderController : Controller
    {
        private readonly MyDbContext _context;
        private const string CART_KEY = "GioHang";

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        // Lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var json = HttpContext.Session.GetString(CART_KEY);
            return string.IsNullOrEmpty(json)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(json)!;
        }

        // Lưu giỏ hàng vào Session
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CART_KEY, JsonConvert.SerializeObject(cart));
        }

        // Trang xác nhận thông tin trước khi đặt (địa chỉ, ghi chú...)
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Msg"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.Total = cart.Sum(x => x.ThanhTien);
            return View(cart);
        }

        // Đặt hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Place(string? ghiChu, string? diaChiGiaoHang)
        {
            var cart = GetCart();
            if (!cart.Any())
            {
                TempData["Msg"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Cart");
            }

            // Tính tổng
            var tong = cart.Sum(x => x.ThanhTien);

            // Lấy user hiện tại
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Tạo Order
            var order = new Order
            {
                UserId = user.Id,
                CreatedAt = DateTime.Now, // Changed from NgayDat to CreatedAt
                TongTien = tong,
                DiaChiGiaoHang = string.IsNullOrWhiteSpace(diaChiGiaoHang) ? user.DiaChi : diaChiGiaoHang,
                GhiChu = ghiChu,
                TrangThai = "Đang xử lý"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo OrderDetails
            foreach (var item in cart)
            {
                _context.OrderDetails.Add(new OrderDetail
                {
                    OrderID = order.OrderID,
                    MaSP = item.MaSP,
                    SoLuong = item.SoLuong,
                    DonGiaLucMua = item.DonGia
                });
            }
            await _context.SaveChangesAsync();

            // Xóa giỏ
            SaveCart(new List<CartItem>());

            // Chuyển sang trang thành công
            return RedirectToAction("Success", new { id = order.OrderID });
        }

        // Trang cảm ơn
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _context.Orders
                .Include(o => o.ChiTiet)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // Lịch sử đơn hàng của tôi
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == User.Identity!.Name);

            if (user == null) return RedirectToPage("/Account/Login", new { area = "Identity" });

            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.CreatedAt) // Changed from NgayDat to CreatedAt
                .ToListAsync();

            return View(orders);
        }
    }
}
