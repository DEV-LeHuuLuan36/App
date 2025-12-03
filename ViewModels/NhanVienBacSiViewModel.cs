using System;
using System.ComponentModel.DataAnnotations;

// SỬA LỖI: Chuyển namespace sang "App.ViewModels"
namespace App.ViewModels
{
    // File này tổng hợp các trường từ NhanVien VÀ BacSi
    // để Form "Thêm Bác Sĩ" có thể điền 1 lần
    public class NhanVienBacSiViewModel
    {
        // === Thông tin chung (Từ NhanVien) ===

        [Display(Name = "Mã Bác Sĩ (Dùng chung Mã NV)")]
        [Required(ErrorMessage = "Mã Bác sĩ là bắt buộc")]
        [StringLength(10)]
        public string MaNhanVien { get; set; }

        [Display(Name = "Họ Tên")]
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Giới Tính")]
        [Required(ErrorMessage = "Giới tính là bắt buộc")]
        public string GioiTinh { get; set; }

        [Display(Name = "Số Điện Thoại")]
        [Phone]
        public string? SoDienThoai { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Địa Chỉ")]
        public string? DiaChi { get; set; }

        [Display(Name = "CCCD")]
        [StringLength(20)]
        public string? CCCD { get; set; }

        [Display(Name = "Trình Độ (Chung)")]
        public string? TrinhDo { get; set; }

        [Display(Name = "Ngày Vào Làm")]
        [DataType(DataType.Date)]
        public DateTime? NgayVaoLam { get; set; }

        public bool TrangThai { get; set; } // Dùng cho Edit


        // === Thông tin riêng (Từ BacSi) ===

        [Display(Name = "Thuộc Khoa")]
        [Required(ErrorMessage = "Khoa là bắt buộc")]
        public string MaKhoa { get; set; }

        [Display(Name = "Chuyên Khoa")]
        public string? ChuyenKhoa { get; set; }

        [Display(Name = "Học Vị")]
        public string? HocVi { get; set; }

        [Display(Name = "Bằng Cấp")]
        public string? BangCap { get; set; }

        [Display(Name = "Kinh Nghiệm (năm)")]
        public int? KinhNghiem { get; set; }
    }
}