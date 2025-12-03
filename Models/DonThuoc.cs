using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class DonThuoc
    {
        public DonThuoc()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public string MaDonThuoc { get; set; } = null!;
        public string MaDieuTri { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public string MaBacSi { get; set; } = null!;
        public string MaThuoc { get; set; } = null!;
        public int SoLuong { get; set; }
        public string? CachDung { get; set; }
        public string? GhiChu { get; set; }
        public DateTime? NgayKeDon { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? ThanhTien { get; set; }

        public virtual DieuTri Ma { get; set; } = null!;
        public virtual Thuoc MaThuocNavigation { get; set; } = null!;
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
