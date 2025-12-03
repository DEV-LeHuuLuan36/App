using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public string MaHoaDon { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public DateTime? NgayLap { get; set; }
        public decimal? TongTien { get; set; }
        public string? TrangThai { get; set; }
        public string? PhuongThucTt { get; set; }
        public string? GhiChu { get; set; }
        public string? MaNhanVien { get; set; }
        public DateTime? NgayThanhToan { get; set; }

        public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
        public virtual NhanVien? MaNhanVienNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
