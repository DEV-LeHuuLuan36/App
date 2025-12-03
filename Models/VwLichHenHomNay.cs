using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwLichHenHomNay
    {
        public string MaLichHen { get; set; } = null!;
        public string? HoTenBenhNhan { get; set; }
        public string? SoDienThoai { get; set; }
        public string? TenBacSi { get; set; }
        public string? TenKhoa { get; set; }
        public DateTime? NgayHen { get; set; }
        public string? LyDoKham { get; set; }
        public string? TrangThai { get; set; }
        public int? SoPhutConLai { get; set; }
    }
}
