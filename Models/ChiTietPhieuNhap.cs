using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class ChiTietPhieuNhap
    {
        public string MaCtphieu { get; set; } = null!;
        public string MaPhieuNhap { get; set; } = null!;
        public string MaThuoc { get; set; } = null!;
        public int SoLuong { get; set; }
        public decimal? DonGiaNhap { get; set; }
        public decimal? ThanhTien { get; set; }
        public DateTime? HanSuDung { get; set; }

        public virtual PhieuNhapThuoc MaPhieuNhapNavigation { get; set; } = null!;
        public virtual Thuoc MaThuocNavigation { get; set; } = null!;
    }
}
