using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class BacSiBenhNhan
    {
        public string MaBacSi { get; set; } = null!;
        public string MaBenhNhan { get; set; } = null!;
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string? VaiTro { get; set; }

        public virtual BacSi MaBacSiNavigation { get; set; } = null!;
        public virtual BenhNhan MaBenhNhanNavigation { get; set; } = null!;
    }
}
