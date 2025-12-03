using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class PhanCong
    {
        public string MaPhanCong { get; set; } = null!;
        public string MaBacSi { get; set; } = null!;
        public string MaYta { get; set; } = null!;
        public string MaPhong { get; set; } = null!;
        public DateTime? NgayPhanCong { get; set; }
        public string? GhiChu { get; set; }
        public string? TrangThai { get; set; }

        public virtual BacSi MaBacSiNavigation { get; set; } = null!;
        public virtual PhongBenh MaPhongNavigation { get; set; } = null!;
        public virtual YTa MaYtaNavigation { get; set; } = null!;
    }
}
