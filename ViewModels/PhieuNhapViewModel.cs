using App.Models;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels
{
    // Đây là ViewModel cho form nhập
    public class PhieuNhapViewModel
    {
        // Thông tin Phiếu Nhập (Cha)
        [Display(Name = "Mã Phiếu Nhập")]
        [Required(ErrorMessage = "Mã phiếu không được trống")]
        public string MaPhieuNhap { get; set; }

        [Display(Name = "Nhân viên Lập phiếu")]
        [Required(ErrorMessage = "Phải chọn nhân viên")]
        public string MaNhanVien { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public string? MaNhaCungCap { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        // Danh sách Chi Tiết Nhập (Con)
        public List<ChiTietNhapViewModel> ChiTietNhap { get; set; } = new List<ChiTietNhapViewModel>();
    }

    // Đây là class đại diện cho 1 dòng trong danh sách nhập
    public class ChiTietNhapViewModel
    {
        [Display(Name = "Mã Thuốc")]
        [Required]
        public string MaThuoc { get; set; }

        [Display(Name = "Số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Display(Name = "Đơn giá nhập")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không được âm")]
        public decimal DonGiaNhap { get; set; }

        [Display(Name = "Hạn sử dụng")]
        public DateOnly? HanSuDung { get; set; }

        // Thuộc tính này chỉ dùng để hiển thị tên thuốc trên form
        public string? TenThuoc { get; set; }
    }
}