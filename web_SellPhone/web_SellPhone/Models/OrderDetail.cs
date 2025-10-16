namespace web_SellPhone.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int MaSP { get; set; }
        public Product Product { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaLucMua { get; set; }
    }
}
