using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class CaTruc
    {
        public CaTruc()
        {
            PhanCas = new HashSet<PhanCa>();
        }

        public string MaCa { get; set; } = null!;
        public string? TenCa { get; set; }
        public TimeSpan? GioBatDau { get; set; }
        public TimeSpan? GioKetThuc { get; set; }
        public decimal? HeSoLuong { get; set; }
        public string? MoTa { get; set; }

        public virtual ICollection<PhanCa> PhanCas { get; set; }
    }
}
