using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_SellPhone.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public NguoiDung User { get; set; }

        public DateTime NgayDat { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TongTien { get; set; }

        public string TrangThai { get; set; } = "Pending";

        public string? DiaChiGiaoHang { get; set; }
        public string? GhiChu { get; set; }

        // Giữ lại 1 thuộc tính duy nhất
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
