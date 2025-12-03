using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class DieuTri
    {
        public DieuTri()
        {
            DonThuocs = new HashSet<DonThuoc>();
        }

        public string MaDieuTri { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public string MaBacSi { get; set; } = null!;
        public DateTime? NgayDieuTri { get; set; }
        public string? NoiDung { get; set; }
        public string? KetQua { get; set; }
        public string? TrangThai { get; set; }

        public virtual BacSi MaBacSiNavigation { get; set; } = null!;
        public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
        public virtual ICollection<DonThuoc> DonThuocs { get; set; }
    }
}
