using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class Thuoc
    {
        public Thuoc()
        {
            ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
            DonThuocs = new HashSet<DonThuoc>();
        }

        public string MaThuoc { get; set; } = null!;
        public string TenThuoc { get; set; } = null!;
        public string? DonViTinh { get; set; }
        public decimal? DonGia { get; set; }
        public int? SoLuongTon { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string? ChongChiDinh { get; set; }
        public string? NhaSanXuat { get; set; }
        public string? NuocSanXuat { get; set; }
        public string? LoaiThuoc { get; set; }
        public bool? TrangThai { get; set; }

        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public virtual ICollection<DonThuoc> DonThuocs { get; set; }
    }
}
