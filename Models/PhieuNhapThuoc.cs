using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class PhieuNhapThuoc
    {
        public PhieuNhapThuoc()
        {
            ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
        }

        public string MaPhieuNhap { get; set; } = null!;
        public DateTime? NgayNhap { get; set; }
        public string MaNhanVien { get; set; } = null!;
        public decimal? TongTien { get; set; }
        public string? GhiChu { get; set; }
        public string? MaNhaCungCap { get; set; }
        public string? TrangThai { get; set; }

        public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }
        public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    }
}
