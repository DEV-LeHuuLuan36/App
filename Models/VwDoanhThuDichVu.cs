using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwDoanhThuDichVu
    {
        public string MaDichVu { get; set; } = null!;
        public string TenDichVu { get; set; } = null!;
        public decimal DonGia { get; set; }
        public int? SoLanSuDung { get; set; }
        public decimal? TongDoanhThu { get; set; }
        public int? Thang { get; set; }
        public int? Nam { get; set; }
    }
}
