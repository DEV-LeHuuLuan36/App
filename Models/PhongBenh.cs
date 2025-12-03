using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class PhongBenh
    {
        public PhongBenh()
        {
            BenhNhans = new HashSet<BenhNhan>();
            PhanCongs = new HashSet<PhanCong>();
            ThietBis = new HashSet<ThietBi>();
            Yta = new HashSet<YTa>();
        }

        public string MaPhong { get; set; } = null!;
        public string? TenPhong { get; set; }
        public string MaKhoa { get; set; } = null!;
        public int? SoGiuong { get; set; }
        public int? SoGiuongTrong { get; set; }
        public int? Tang { get; set; }
        public string? LoaiPhong { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Khoa MaKhoaNavigation { get; set; } = null!;
        public virtual ICollection<BenhNhan> BenhNhans { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
        public virtual ICollection<ThietBi> ThietBis { get; set; }
        public virtual ICollection<YTa> Yta { get; set; }
    }
}
