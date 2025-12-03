using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwBenhNhanCanTheoDoi
    {
        public string MaBenhNhan { get; set; } = null!;
        public string? HoTenBenhNhan { get; set; }
        public string? TenPhong { get; set; }
        public string? TenKhoa { get; set; }
        public string? TenBacSi { get; set; }
        public string? TinhTrang { get; set; }
        public string? ChuanDoan { get; set; }
        public DateTime? NgayNhapVien { get; set; }
        public int? SoNgayNamVien { get; set; }
        public decimal? NhietDo { get; set; }
        public string? HuyetAp { get; set; }
        public int? NhipTim { get; set; }
    }
}
