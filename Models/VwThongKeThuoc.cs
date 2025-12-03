using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwThongKeThuoc
    {
        public string MaThuoc { get; set; } = null!;
        public string TenThuoc { get; set; } = null!;
        public string? DonViTinh { get; set; }
        public int? SoLuongTon { get; set; }
        public decimal? DonGia { get; set; }
        public DateTime? HanSuDung { get; set; }
        public int SoLuongDaBan { get; set; }
        public decimal DoanhThuThuoc { get; set; }
        public string TinhTrang { get; set; } = null!;
    }
}
