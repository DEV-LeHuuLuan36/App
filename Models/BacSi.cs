using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class BacSi
    {
        public BacSi()
        {
            BacSiBenhNhans = new HashSet<BacSiBenhNhan>();
            DieuTris = new HashSet<DieuTri>();
            HoSoBenhAns = new HashSet<HoSoBenhAn>();
            LichHens = new HashSet<LichHen>();
            PhanCongs = new HashSet<PhanCong>();
        }

        public string MaBacSi { get; set; } = null!;
        public string? TenBacSi { get; set; }
        public string MaKhoa { get; set; } = null!;
        public string? ChuyenKhoa { get; set; }
        public string? HocVi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public string? BangCap { get; set; }
        public int? KinhNghiem { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Khoa MaKhoaNavigation { get; set; } = null!;
        public virtual ICollection<BacSiBenhNhan> BacSiBenhNhans { get; set; }
        public virtual ICollection<DieuTri> DieuTris { get; set; }
        public virtual ICollection<HoSoBenhAn> HoSoBenhAns { get; set; }
        public virtual ICollection<LichHen> LichHens { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
    }
}
