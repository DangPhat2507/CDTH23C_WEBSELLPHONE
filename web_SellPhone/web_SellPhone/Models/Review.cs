namespace web_SellPhone.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int MaSP { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public NguoiDung User { get; set; }
        public int Rating { get; set; }
        public string NoiDung { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool DaDuyet { get; set; } = false;
    }
}
