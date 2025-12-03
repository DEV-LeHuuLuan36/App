using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class ChiTietHoaDon
    {
        public string MaCthd { get; set; } = null!;
        public string MaHoaDon { get; set; } = null!;
        public string? MaDichVu { get; set; }
        public string? MaDonThuoc { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? ThanhTien { get; set; }
        public string? Loai { get; set; }

        public virtual DichVu? MaDichVuNavigation { get; set; }
        public virtual DonThuoc? MaDonThuocNavigation { get; set; }
        public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;
    }
}
