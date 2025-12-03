using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class LichHen
    {
        public string MaLichHen { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public string? MaBacSi { get; set; }
        public string MaKhoa { get; set; } = null!;
        public DateTime? NgayHen { get; set; }
        public string? LyDoKham { get; set; }
        public string? TrangThai { get; set; }
        public string? GhiChu { get; set; }
        public DateTime? NgayTao { get; set; }

        public virtual BacSi? MaBacSiNavigation { get; set; }
        public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
        public virtual Khoa MaKhoaNavigation { get; set; } = null!;
    }
}
