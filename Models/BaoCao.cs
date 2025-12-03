using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class BaoCao
    {
        public string MaBaoCao { get; set; } = null!;
        public string? LoaiBaoCao { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string? NoiDung { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? NguoiTao { get; set; }
        public string? TrangThai { get; set; }
        public string? NguoiDuyet { get; set; }
        public DateTime? NgayDuyet { get; set; }

        public virtual NhanVien? NguoiDuyetNavigation { get; set; }
        public virtual NhanVien? NguoiTaoNavigation { get; set; }
    }
}
