using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class Khoa
    {
        public Khoa()
        {
            BacSis = new HashSet<BacSi>();
            DichVus = new HashSet<DichVu>();
            LichHens = new HashSet<LichHen>();
            NhanViens = new HashSet<NhanVien>();
            PhongBenhs = new HashSet<PhongBenh>();
        }

        public string MaKhoa { get; set; } = null!;
        public string? TenKhoa { get; set; }
        public string? MoTa { get; set; }
        public string? TruongKhoa { get; set; }
        public DateTime? NgayThanhLap { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<BacSi> BacSis { get; set; }
        public virtual ICollection<DichVu> DichVus { get; set; }
        public virtual ICollection<LichHen> LichHens { get; set; }
        public virtual ICollection<NhanVien> NhanViens { get; set; }
        public virtual ICollection<PhongBenh> PhongBenhs { get; set; }
    }
}
