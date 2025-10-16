using Microsoft.AspNetCore.Identity;

namespace web_SellPhone.Models
{
    public class NguoiDung : IdentityUser
    {
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public bool BiKhoa { get; set; } = false;
    }
}
