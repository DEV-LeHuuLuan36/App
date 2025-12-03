namespace App.ViewModels
{
    public class NhanVienYTaViewModel
    {
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? CCCD { get; set; }
        public string? TrinhDo { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public bool TrangThai { get; set; }

        // Từ YTa
        public string MaPhong { get; set; }
        public int? KinhNghiem { get; set; }
    }
}
