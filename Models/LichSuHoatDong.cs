using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class LichSuHoatDong
    {
        public string MaLichSu { get; set; } = null!;
        public string? MaNhanVien { get; set; }
        public string? HanhDong { get; set; }
        public string? BangTacDong { get; set; }
        public string? MaBanGhi { get; set; }
        public string? NoiDungTruoc { get; set; }
        public string? NoiDungSau { get; set; }
        public DateTime? ThoiGian { get; set; }
        public string? DiaChiIp { get; set; }
        public string? MayTinh { get; set; }
        public string? TrinhDuyet { get; set; }

        public virtual NhanVien? MaNhanVienNavigation { get; set; }
    }
}
