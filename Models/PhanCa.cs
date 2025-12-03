using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class PhanCa
    {
        public string MaPhanCa { get; set; } = null!;
        public string? MaNhanVien { get; set; }
        public string? MaCa { get; set; }
        public DateTime? NgayLamViec { get; set; }
        public string? GhiChu { get; set; }
        public string? TrangThai { get; set; }

        public virtual CaTruc? MaCaNavigation { get; set; }
        public virtual NhanVien? MaNhanVienNavigation { get; set; }
    }
}
