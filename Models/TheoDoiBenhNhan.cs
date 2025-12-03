using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class TheoDoiBenhNhan
    {
        public string MaTheoDoi { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public string MaYTa { get; set; } = null!;
        public DateTime? NgayTheoDoi { get; set; }
        public decimal? NhietDo { get; set; }
        public string? HuyetAp { get; set; }
        public int? NhipTim { get; set; }
        public decimal? CanNang { get; set; }
        public decimal? ChieuCao { get; set; }

        public int? NhietDoHoiTho { get; set; }
        public string? ChiSoKhac { get; set; }
        public string? GhiChu { get; set; }
        public string? DanhGia { get; set; }

        public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
        public virtual YTa MaYtaNavigation { get; set; } = null!;
    }
}
