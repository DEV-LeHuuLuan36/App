using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwThongKeThietBi
    {
        public string MaThietBi { get; set; } = null!;
        public string TenThietBi { get; set; } = null!;
        public string? TenPhong { get; set; }
        public string? TenKhoa { get; set; }
        public DateTime? NgayMua { get; set; }
        public DateTime? HanBaoHanh { get; set; }
        public string? TinhTrang { get; set; }
        public int? SoThangConBaoHanh { get; set; }
        public string TrangThaiChiTiet { get; set; } = null!;
    }
}
