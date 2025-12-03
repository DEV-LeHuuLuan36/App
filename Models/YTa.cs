using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class YTa
    {
        public YTa()
        {
            PhanCongs = new HashSet<PhanCong>();
            TheoDoiBenhNhans = new HashSet<TheoDoiBenhNhan>();
        }

        public string MaYTa { get; set; } = null!;
        public string? TenYTa { get; set; }
        public string MaPhong { get; set; } = null!;
        public string? TrinhDo { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public int? KinhNghiem { get; set; }
        public bool? TrangThai { get; set; }

        public virtual PhongBenh MaPhongNavigation { get; set; } = null!;
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
        public virtual ICollection<TheoDoiBenhNhan> TheoDoiBenhNhans { get; set; }
    }
}
