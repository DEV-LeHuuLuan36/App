using System;
using System.Collections.Generic;

namespace App.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            BaoCaoNguoiDuyetNavigations = new HashSet<BaoCao>();
            BaoCaoNguoiTaoNavigations = new HashSet<BaoCao>();
            HoaDons = new HashSet<HoaDon>();
            LichLamViecs = new HashSet<LichLamViec>();
            LichSuHoatDongs = new HashSet<LichSuHoatDong>();
            PhanCas = new HashSet<PhanCa>();
            PhieuNhapThuocs = new HashSet<PhieuNhapThuoc>();
            ThietBis = new HashSet<ThietBi>();
        }

        public string MaNhanVien { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? Cccd { get; set; }
        public string? ChucVu { get; set; }
        public string? TrinhDo { get; set; }
        public DateTime? NgayVaoLam { get; set; }
        public string? MaKhoa { get; set; }
        public string? LoaiNhanVien { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Khoa? MaKhoaNavigation { get; set; }
        public virtual ICollection<BaoCao> BaoCaoNguoiDuyetNavigations { get; set; }
        public virtual ICollection<BaoCao> BaoCaoNguoiTaoNavigations { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<LichLamViec> LichLamViecs { get; set; }
        public virtual ICollection<LichSuHoatDong> LichSuHoatDongs { get; set; }
        public virtual ICollection<PhanCa> PhanCas { get; set; }
        public virtual ICollection<PhieuNhapThuoc> PhieuNhapThuocs { get; set; }
        public virtual ICollection<ThietBi> ThietBis { get; set; }
    }
}
