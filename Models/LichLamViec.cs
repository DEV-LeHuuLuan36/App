using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class LichLamViec
    {
        public string MaLich { get; set; } = null!;
        public string MaNhanVien { get; set; } = null!;
        public string? LoaiNhanVien { get; set; }
        public DateTime? NgayLamViec { get; set; }
        public string? CaLamViec { get; set; }
        public string? GhiChu { get; set; }
        public string? TrangThai { get; set; }

        public virtual NhanVien MaNhanVienNavigation { get; set; } = null!;
    }
}
