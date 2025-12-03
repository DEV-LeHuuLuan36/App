using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwLichLamViecNhanVien
    {
        public string MaLich { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string? ChucVu { get; set; }
        public string? LoaiNhanVien { get; set; }
        public DateTime? NgayLamViec { get; set; }
        public string? CaLamViec { get; set; }
        public string? GhiChu { get; set; }
        public string? TenKhoa { get; set; }
        public string? TenPhong { get; set; }
    }
}
