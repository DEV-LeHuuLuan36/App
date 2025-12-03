using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class BenhNhan
    {
        public BenhNhan()
        {
            BacSiBenhNhans = new HashSet<BacSiBenhNhan>();
            DieuTris = new HashSet<DieuTri>();
            HoSoBenhAns = new HashSet<HoSoBenhAn>();
            HoaDons = new HashSet<HoaDon>();
            LichHens = new HashSet<LichHen>();
            TheoDoiBenhNhans = new HashSet<TheoDoiBenhNhan>();
        }

        public string MaBenhNhan { get; set; } = null!;
        public string? HoTenBenhNhan { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? Cccd { get; set; }
        public DateTime? NgayNhapVien { get; set; }
        public DateTime? NgayXuatVien { get; set; }
        public string MaPhong { get; set; } = null!;
        public string? TrangThai { get; set; }
        public string? NguoiThan { get; set; }
        public string? SdtnguoiThan { get; set; }

        public virtual PhongBenh MaPhongNavigation { get; set; } = null!;
        public virtual ICollection<BacSiBenhNhan> BacSiBenhNhans { get; set; }
        public virtual ICollection<DieuTri> DieuTris { get; set; }
        public virtual ICollection<HoSoBenhAn> HoSoBenhAns { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<LichHen> LichHens { get; set; }
        public virtual ICollection<TheoDoiBenhNhan> TheoDoiBenhNhans { get; set; }
    }
}
