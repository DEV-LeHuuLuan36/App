using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class DichVu
    {
        public DichVu()
        {
            ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }

        public string MaDichVu { get; set; } = null!;
        public string TenDichVu { get; set; } = null!;
        public decimal DonGia { get; set; }
        public string? MoTa { get; set; }
        public string? MaKhoa { get; set; }
        public int? ThoiGianThucHien { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Khoa? MaKhoaNavigation { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
    }
}
