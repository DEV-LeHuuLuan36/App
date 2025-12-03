using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class VwBaoCaoBenhTheoKhoa
    {
        public string MaKhoa { get; set; } = null!;
        public string? TenKhoa { get; set; }
        public string? ChuanDoan { get; set; }
        public int? SoLuong { get; set; }
        public int? TuoiTrungBinh { get; set; }
        public int? SoNam { get; set; }
        public int? SoNu { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }
}
