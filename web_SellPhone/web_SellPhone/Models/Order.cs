namespace web_SellPhone.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string UserId { get; set; }
        public NguoiDung User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = "Pending";
        public ICollection<OrderDetail> ChiTiet { get; set; }
    }
}
