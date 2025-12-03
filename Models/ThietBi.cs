using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class ThietBi
    {
        public string MaThietBi { get; set; } = null!;
        public string TenThietBi { get; set; } = null!;
        public string? MaPhong { get; set; }
        public DateTime? NgayMua { get; set; }
        public DateTime? HanBaoHanh { get; set; }
        public string? TinhTrang { get; set; }
        public string? GhiChu { get; set; }
        public decimal? DonGia { get; set; }
        public string? PhongBan { get; set; }
        public DateTime? NgayKiemKe { get; set; }
        public string? NguoiQuanLy { get; set; }

        public virtual PhongBenh? MaPhongNavigation { get; set; }
        public virtual NhanVien? NguoiQuanLyNavigation { get; set; }
    }
}
