using System.ComponentModel.DataAnnotations;

namespace web_SellPhone.Models
{
    public class Product
    {
        [Key]
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal DonGia { get; set; }
        public decimal? DonGiaKhuyenMai { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
