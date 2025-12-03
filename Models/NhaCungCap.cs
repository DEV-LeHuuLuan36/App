using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class NhaCungCap
    {
        public NhaCungCap()
        {
            PhieuNhapThuocs = new HashSet<PhieuNhapThuoc>();
        }

        public string MaNcc { get; set; } = null!;
        public string TenNcc { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? MaSoThue { get; set; }
        public string? NguoiDaiDien { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<PhieuNhapThuoc> PhieuNhapThuocs { get; set; }
    }
}
