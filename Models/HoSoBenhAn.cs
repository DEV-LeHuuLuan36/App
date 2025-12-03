using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class HoSoBenhAn
    {
        public string MaHs { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public DateTime? NgayLap { get; set; }
        public string? ChuanDoan { get; set; }
        public string? TinhTrang { get; set; }
        public string? TrieuChung { get; set; }
        public string? TienSuBenh { get; set; }
        public string? DiUng { get; set; }
        public DateTime? NgayTaiKham { get; set; }
        public string MaBacSi { get; set; } = null!;

        public virtual BacSi MaBacSiNavigation { get; set; } = null!;
        public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
    }
}
