using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace App.Models
{
    public partial class QLBenhVienContext : DbContext
    {
        public QLBenhVienContext()
        {
        }

        public QLBenhVienContext(DbContextOptions<QLBenhVienContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BacSi> BacSis { get; set; } = null!;
        public virtual DbSet<BacSiBenhNhan> BacSiBenhNhans { get; set; } = null!;
        public virtual DbSet<BaoCao> BaoCaos { get; set; } = null!;
        public virtual DbSet<BenhNhan> BenhNhans { get; set; } = null!;
        public virtual DbSet<CaTruc> CaTrucs { get; set; } = null!;
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; } = null!;
        public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = null!;
        public virtual DbSet<DichVu> DichVus { get; set; } = null!;
        public virtual DbSet<DieuTri> DieuTris { get; set; } = null!;
        public virtual DbSet<DonThuoc> DonThuocs { get; set; } = null!;
        public virtual DbSet<HoSoBenhAn> HoSoBenhAns { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<Khoa> Khoas { get; set; } = null!;
        public virtual DbSet<LichHen> LichHens { get; set; } = null!;
        public virtual DbSet<LichLamViec> LichLamViecs { get; set; } = null!;
        public virtual DbSet<LichSuHoatDong> LichSuHoatDongs { get; set; } = null!;
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhanCa> PhanCas { get; set; } = null!;
        public virtual DbSet<PhanCong> PhanCongs { get; set; } = null!;
        public virtual DbSet<PhieuNhapThuoc> PhieuNhapThuocs { get; set; } = null!;
        public virtual DbSet<PhongBenh> PhongBenhs { get; set; } = null!;
        public virtual DbSet<TheoDoiBenhNhan> TheoDoiBenhNhans { get; set; } = null!;
        public virtual DbSet<ThietBi> ThietBis { get; set; } = null!;
        public virtual DbSet<Thuoc> Thuocs { get; set; } = null!;
        public virtual DbSet<UserLock> UserLocks { get; set; } = null!;
        public virtual DbSet<VwBaoCaoBenhTheoKhoa> VwBaoCaoBenhTheoKhoas { get; set; } = null!;
        public virtual DbSet<VwBenhNhanCanTheoDoi> VwBenhNhanCanTheoDois { get; set; } = null!;
        public virtual DbSet<VwDoanhThuDichVu> VwDoanhThuDichVus { get; set; } = null!;
        public virtual DbSet<VwDoanhThuTheoThang> VwDoanhThuTheoThangs { get; set; } = null!;
        public virtual DbSet<VwLichHenHomNay> VwLichHenHomNays { get; set; } = null!;
        public virtual DbSet<VwLichLamViecNhanVien> VwLichLamViecNhanViens { get; set; } = null!;
        public virtual DbSet<VwThongKeBenhNhan> VwThongKeBenhNhans { get; set; } = null!;
        public virtual DbSet<VwThongKeHoatDong> VwThongKeHoatDongs { get; set; } = null!;
        public virtual DbSet<VwThongKeThietBi> VwThongKeThietBis { get; set; } = null!;
        public virtual DbSet<VwThongKeThuoc> VwThongKeThuocs { get; set; } = null!;
        public virtual DbSet<VwThuocSapHetHan> VwThuocSapHetHans { get; set; } = null!;
        public virtual DbSet<YTa> Yta { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\SQLLUAN;Database=QLBenhVien;User Id=admin_luan;Password=1;Trusted_Connection=False;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BacSi>(entity =>
            {
                entity.HasKey(e => e.MaBacSi)
                    .HasName("PK__BacSi__E022715E91458910");

                entity.ToTable("BacSi");

                entity.HasIndex(e => e.MaKhoa, "IX_BacSi_MaKhoa");

                entity.HasIndex(e => e.Cccd, "UQ__BacSi__A955A0AA9227ACDD")
                    .IsUnique();

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BangCap).HasMaxLength(200);

                entity.Property(e => e.Cccd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CCCD");

                entity.Property(e => e.ChuyenKhoa).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HocVi).HasMaxLength(50);

                entity.Property(e => e.KinhNghiem).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayVaoLam)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TenBacSi).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaKhoaNavigation)
                    .WithMany(p => p.BacSis)
                    .HasForeignKey(d => d.MaKhoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BacSi__MaKhoa__5EBF139D");
            });

            modelBuilder.Entity<BacSiBenhNhan>(entity =>
            {
                entity.HasKey(e => new { e.MaBacSi, e.MaBenhNhan, e.NgayBatDau })
                    .HasName("PK__BacSi_Be__A3B4FED8CB35D154");

                entity.ToTable("BacSi_BenhNhan");

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayBatDau)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgayKetThuc).HasColumnType("date");

                entity.Property(e => e.VaiTro).HasMaxLength(50);

                entity.HasOne(d => d.MaBacSiNavigation)
                    .WithMany(p => p.BacSiBenhNhans)
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BacSi_Ben__MaBac__6166761E");

                entity.HasOne(d => d.MaBenhNhanNavigation)
                    .WithMany(p => p.BacSiBenhNhans)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BacSi_Ben__MaBen__625A9A57");
            });

            modelBuilder.Entity<BaoCao>(entity =>
            {
                entity.HasKey(e => e.MaBaoCao)
                    .HasName("PK__BaoCao__25A9188CCDE869E1");

                entity.ToTable("BaoCao");

                entity.Property(e => e.MaBaoCao)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DenNgay).HasColumnType("date");

                entity.Property(e => e.LoaiBaoCao).HasMaxLength(50);

                entity.Property(e => e.NgayDuyet).HasColumnType("datetime");

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NguoiDuyet)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NguoiTao)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đã tạo')");

                entity.Property(e => e.TuNgay).HasColumnType("date");

                entity.HasOne(d => d.NguoiDuyetNavigation)
                    .WithMany(p => p.BaoCaoNguoiDuyetNavigations)
                    .HasForeignKey(d => d.NguoiDuyet)
                    .HasConstraintName("FK__BaoCao__NguoiDuy__57DD0BE4");

                entity.HasOne(d => d.NguoiTaoNavigation)
                    .WithMany(p => p.BaoCaoNguoiTaoNavigations)
                    .HasForeignKey(d => d.NguoiTao)
                    .HasConstraintName("FK__BaoCao__NguoiTao__55009F39");
            });

            modelBuilder.Entity<BenhNhan>(entity =>
            {
                entity.HasKey(e => e.MaBenhNhan)
                    .HasName("PK__BenhNhan__22A8B3303D37FD1F");

                entity.ToTable("BenhNhan");

                entity.HasIndex(e => e.MaPhong, "IX_BenhNhan_MaPhong");

                entity.HasIndex(e => e.NgayNhapVien, "IX_BenhNhan_NgayNhapVien");

                entity.HasIndex(e => e.Cccd, "UQ__BenhNhan__A955A0AA6EAEE423")
                    .IsUnique();

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cccd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CCCD");

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GioiTinh).HasMaxLength(10);

                entity.Property(e => e.HoTenBenhNhan).HasMaxLength(100);

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayNhapVien)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.NgayXuatVien).HasColumnType("date");

                entity.Property(e => e.NguoiThan).HasMaxLength(100);

                entity.Property(e => e.SdtnguoiThan)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("SDTNguoiThan");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang điều trị')");

                entity.HasOne(d => d.MaPhongNavigation)
                    .WithMany(p => p.BenhNhans)
                    .HasForeignKey(d => d.MaPhong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BenhNhan__MaPhon__5812160E");
            });

            modelBuilder.Entity<CaTruc>(entity =>
            {
                entity.HasKey(e => e.MaCa)
                    .HasName("PK__CaTruc__27258E7B2BB170A0");

                entity.ToTable("CaTruc");

                entity.Property(e => e.MaCa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HeSoLuong)
                    .HasColumnType("decimal(3, 2)")
                    .HasDefaultValueSql("((1.0))");

                entity.Property(e => e.MoTa).HasMaxLength(200);

                entity.Property(e => e.TenCa).HasMaxLength(50);
            });

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.HasKey(e => e.MaCthd)
                    .HasName("PK__ChiTietH__1E4FA771E3EA3BDB");

                entity.ToTable("ChiTietHoaDon");

                entity.HasIndex(e => e.MaHoaDon, "IX_ChiTietHoaDon_MaHoaDon");

                entity.Property(e => e.MaCthd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaCTHD")
                    .IsFixedLength();

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.Loai).HasMaxLength(20);

                entity.Property(e => e.MaDichVu)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaDonThuoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaHoaDon)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SoLuong).HasDefaultValueSql("((1))");

                entity.Property(e => e.ThanhTien).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaDichVuNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaDichVu)
                    .HasConstraintName("FK__ChiTietHo__MaDic__208CD6FA");

                entity.HasOne(d => d.MaDonThuocNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaDonThuoc)
                    .HasConstraintName("FK__ChiTietHo__MaDon__2180FB33");

                entity.HasOne(d => d.MaHoaDonNavigation)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHoaDon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietHo__MaHoa__1F98B2C1");
            });

            modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
            {
                entity.HasKey(e => e.MaCtphieu)
                    .HasName("PK__ChiTietP__0C8D8D3284C4466B");

                entity.ToTable("ChiTietPhieuNhap");

                entity.HasIndex(e => e.MaPhieuNhap, "IX_ChiTietPhieuNhap_MaPhieuNhap");

                entity.Property(e => e.MaCtphieu)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaCTPhieu")
                    .IsFixedLength();

                entity.Property(e => e.DonGiaNhap).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.HanSuDung).HasColumnType("date");

                entity.Property(e => e.MaPhieuNhap)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaThuoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ThanhTien).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaPhieuNhapNavigation)
                    .WithMany(p => p.ChiTietPhieuNhaps)
                    .HasForeignKey(d => d.MaPhieuNhap)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietPh__MaPhi__4E53A1AA");

                entity.HasOne(d => d.MaThuocNavigation)
                    .WithMany(p => p.ChiTietPhieuNhaps)
                    .HasForeignKey(d => d.MaThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietPh__MaThu__4F47C5E3");
            });

            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.HasKey(e => e.MaDichVu)
                    .HasName("PK__DichVu__C0E6DE8F8817BDF3");

                entity.ToTable("DichVu");

                entity.Property(e => e.MaDichVu)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(300);

                entity.Property(e => e.TenDichVu).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaKhoaNavigation)
                    .WithMany(p => p.DichVus)
                    .HasForeignKey(d => d.MaKhoa)
                    .HasConstraintName("FK__DichVu__MaKhoa__0B91BA14");
            });

            modelBuilder.Entity<DieuTri>(entity =>
            {
                entity.HasKey(e => new { e.MaDieuTri, e.MaBenhNhan, e.MaBacSi })
                    .HasName("PK__DieuTri__FB3CC2069CA05F08");

                entity.ToTable("DieuTri");

                entity.HasIndex(e => e.MaBacSi, "IX_DieuTri_MaBacSi");

                entity.HasIndex(e => e.MaBenhNhan, "IX_DieuTri_MaBenhNhan");

                entity.Property(e => e.MaDieuTri)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.KetQua).HasMaxLength(200);

                entity.Property(e => e.NgayDieuTri)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NoiDung).HasMaxLength(300);

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang điều trị')");

                entity.HasOne(d => d.MaBacSiNavigation)
                    .WithMany(p => p.DieuTris)
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DieuTri__MaBacSi__70DDC3D8");

                entity.HasOne(d => d.MaBenhNhanNavigation)
                    .WithMany(p => p.DieuTris)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DieuTri__MaBenhN__6FE99F9F");
            });

            modelBuilder.Entity<DonThuoc>(entity =>
            {
                entity.HasKey(e => e.MaDonThuoc)
                    .HasName("PK__DonThuoc__3EF99EE139BFC08F");

                entity.ToTable("DonThuoc");

                entity.HasIndex(e => new { e.MaDieuTri, e.MaBenhNhan, e.MaBacSi }, "IX_DonThuoc_MaDieuTri");

                entity.Property(e => e.MaDonThuoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CachDung).HasMaxLength(200);

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.GhiChu).HasMaxLength(300);

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaDieuTri)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaThuoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayKeDon)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ThanhTien).HasColumnType("decimal(15, 2)");

                entity.HasOne(d => d.MaThuocNavigation)
                    .WithMany(p => p.DonThuocs)
                    .HasForeignKey(d => d.MaThuoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DonThuoc__MaThuo__05D8E0BE");

                entity.HasOne(d => d.Ma)
                    .WithMany(p => p.DonThuocs)
                    .HasForeignKey(d => new { d.MaDieuTri, d.MaBenhNhan, d.MaBacSi })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DonThuoc__07C12930");
            });

            modelBuilder.Entity<HoSoBenhAn>(entity =>
            {
                entity.HasKey(e => new { e.MaHs, e.MaBenhNhan })
                    .HasName("PK__HoSoBenh__350F2DDC9D095705");

                entity.ToTable("HoSoBenhAn");

                entity.HasIndex(e => e.MaBenhNhan, "IX_HoSoBenhAn_MaBenhNhan");

                entity.Property(e => e.MaHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaHS")
                    .IsFixedLength();

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ChuanDoan).HasMaxLength(200);

                entity.Property(e => e.DiUng).HasMaxLength(200);

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayLap)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgayTaiKham).HasColumnType("date");

                entity.Property(e => e.TienSuBenh).HasMaxLength(500);

                entity.Property(e => e.TinhTrang).HasMaxLength(100);

                entity.Property(e => e.TrieuChung).HasMaxLength(500);

                entity.HasOne(d => d.MaBacSiNavigation)
                    .WithMany(p => p.HoSoBenhAns)
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoSoBenhA__MaBac__6D0D32F4");

                entity.HasOne(d => d.MaBenhNhanNavigation)
                    .WithMany(p => p.HoSoBenhAns)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoSoBenhA__MaBen__6B24EA82");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.MaHoaDon)
                    .HasName("PK__HoaDon__835ED13BAEB0CB52");

                entity.ToTable("HoaDon");

                entity.HasIndex(e => e.MaBenhNhan, "IX_HoaDon_MaBenhNhan");

                entity.HasIndex(e => e.NgayLap, "IX_HoaDon_NgayLap");

                entity.Property(e => e.MaHoaDon)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GhiChu).HasMaxLength(300);

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayLap)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");

                entity.Property(e => e.PhuongThucTt)
                    .HasMaxLength(50)
                    .HasColumnName("PhuongThucTT");

                entity.Property(e => e.TongTien)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Chưa thanh toán')");

                entity.HasOne(d => d.MaBenhNhanNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoaDon__MaBenhNh__17036CC0");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__HoaDon__MaNhanVi__1BC821DD");
            });

            modelBuilder.Entity<Khoa>(entity =>
            {
                entity.HasKey(e => e.MaKhoa)
                    .HasName("PK__Khoa__65390405AE707B71");

                entity.ToTable("Khoa");

                entity.HasIndex(e => e.TenKhoa, "UQ__Khoa__AAD361581FCC2D98")
                    .IsUnique();

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(200);

                entity.Property(e => e.NgayThanhLap)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TenKhoa).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.Property(e => e.TruongKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<LichHen>(entity =>
            {
                entity.HasKey(e => e.MaLichHen)
                    .HasName("PK__LichHen__150F264F4E4660F1");

                entity.ToTable("LichHen");

                entity.HasIndex(e => e.MaBenhNhan, "IX_LichHen_MaBenhNhan");

                entity.HasIndex(e => e.NgayHen, "IX_LichHen_NgayHen");

                entity.Property(e => e.MaLichHen)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GhiChu).HasMaxLength(300);

                entity.Property(e => e.LyDoKham).HasMaxLength(200);

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayHen).HasColumnType("datetime");

                entity.Property(e => e.NgayTao)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Chờ xác nhận')");

                entity.HasOne(d => d.MaBacSiNavigation)
                    .WithMany(p => p.LichHens)
                    .HasForeignKey(d => d.MaBacSi)
                    .HasConstraintName("FK__LichHen__MaBacSi__778AC167");

                entity.HasOne(d => d.MaBenhNhanNavigation)
                    .WithMany(p => p.LichHens)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LichHen__MaBenhN__76969D2E");

                entity.HasOne(d => d.MaKhoaNavigation)
                    .WithMany(p => p.LichHens)
                    .HasForeignKey(d => d.MaKhoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LichHen__MaKhoa__787EE5A0");
            });

            modelBuilder.Entity<LichLamViec>(entity =>
            {
                entity.HasKey(e => e.MaLich)
                    .HasName("PK__LichLamV__728A9AE950416D65");

                entity.ToTable("LichLamViec");

                entity.Property(e => e.MaLich)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CaLamViec).HasMaxLength(20);

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.LoaiNhanVien).HasMaxLength(10);

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayLamViec).HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đã lên lịch')");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.LichLamViecs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LichLamVi__MaNha__2DE6D218");
            });

            modelBuilder.Entity<LichSuHoatDong>(entity =>
            {
                entity.HasKey(e => e.MaLichSu)
                    .HasName("PK__LichSuHo__C443222AD8098653");

                entity.ToTable("LichSuHoatDong");

                entity.Property(e => e.MaLichSu)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BangTacDong).HasMaxLength(50);

                entity.Property(e => e.DiaChiIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DiaChiIP");

                entity.Property(e => e.HanhDong).HasMaxLength(100);

                entity.Property(e => e.MaBanGhi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MayTinh).HasMaxLength(100);

                entity.Property(e => e.ThoiGian)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrinhDuyet).HasMaxLength(100);

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.LichSuHoatDongs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__LichSuHoa__MaNha__5AB9788F");
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNcc)
                    .HasName("PK__NhaCungC__3A185DEBE3B26250");

                entity.ToTable("NhaCungCap");

                entity.Property(e => e.MaNcc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaNCC")
                    .IsFixedLength();

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MaSoThue)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NguoiDaiDien).HasMaxLength(100);

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TenNcc)
                    .HasMaxLength(100)
                    .HasColumnName("TenNCC");

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaNhanVien)
                    .HasName("PK__NhanVien__77B2CA4706335192");

                entity.ToTable("NhanVien");

                entity.HasIndex(e => e.Cccd, "UQ__NhanVien__A955A0AA558FD41E")
                    .IsUnique();

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Cccd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CCCD");

                entity.Property(e => e.ChucVu).HasMaxLength(50);

                entity.Property(e => e.DiaChi).HasMaxLength(200);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GioiTinh).HasMaxLength(10);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.LoaiNhanVien).HasMaxLength(20);

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgaySinh).HasColumnType("date");

                entity.Property(e => e.NgayVaoLam).HasColumnType("date");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.Property(e => e.TrinhDo).HasMaxLength(100);

                entity.HasOne(d => d.MaKhoaNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.MaKhoa)
                    .HasConstraintName("FK__NhanVien__MaKhoa__123EB7A3");
            });

            modelBuilder.Entity<PhanCa>(entity =>
            {
                entity.HasKey(e => e.MaPhanCa)
                    .HasName("PK__PhanCa__6EEE7F27DBD74D43");

                entity.ToTable("PhanCa");

                entity.Property(e => e.MaPhanCa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.MaCa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayLamViec).HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đã phân')");

                entity.HasOne(d => d.MaCaNavigation)
                    .WithMany(p => p.PhanCas)
                    .HasForeignKey(d => d.MaCa)
                    .HasConstraintName("FK__PhanCa__MaCa__6AEFE058");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.PhanCas)
                    .HasForeignKey(d => d.MaNhanVien)
                    .HasConstraintName("FK__PhanCa__MaNhanVi__69FBBC1F");
            });

            modelBuilder.Entity<PhanCong>(entity =>
            {
                entity.HasKey(e => e.MaPhanCong)
                    .HasName("PK__PhanCong__C279D916B79708EF");

                entity.ToTable("PhanCong");

                entity.Property(e => e.MaPhanCong)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.MaBacSi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaYta)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaYTa")
                    .IsFixedLength();

                entity.Property(e => e.NgayPhanCong)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang làm')");

                entity.HasOne(d => d.MaBacSiNavigation)
                    .WithMany(p => p.PhanCongs)
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhanCong__MaBacS__339FAB6E");

                entity.HasOne(d => d.MaPhongNavigation)
                    .WithMany(p => p.PhanCongs)
                    .HasForeignKey(d => d.MaPhong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhanCong__MaPhon__3587F3E0");

                entity.HasOne(d => d.MaYtaNavigation)
                    .WithMany(p => p.PhanCongs)
                    .HasForeignKey(d => d.MaYta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhanCong__MaYTa__3493CFA7");
            });

            modelBuilder.Entity<PhieuNhapThuoc>(entity =>
            {
                entity.HasKey(e => e.MaPhieuNhap)
                    .HasName("PK__PhieuNha__1470EF3B9F9D0039");

                entity.ToTable("PhieuNhapThuoc");

                entity.HasIndex(e => e.NgayNhap, "IX_PhieuNhapThuoc_NgayNhap");

                entity.Property(e => e.MaPhieuNhap)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GhiChu).HasMaxLength(300);

                entity.Property(e => e.MaNhaCungCap)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaNhanVien)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayNhap)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TongTien)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đã nhập')");

                entity.HasOne(d => d.MaNhaCungCapNavigation)
                    .WithMany(p => p.PhieuNhapThuocs)
                    .HasForeignKey(d => d.MaNhaCungCap)
                    .HasConstraintName("FK__PhieuNhap__MaNha__498EEC8D");

                entity.HasOne(d => d.MaNhanVienNavigation)
                    .WithMany(p => p.PhieuNhapThuocs)
                    .HasForeignKey(d => d.MaNhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhieuNhap__MaNha__47A6A41B");
            });

            modelBuilder.Entity<PhongBenh>(entity =>
            {
                entity.HasKey(e => e.MaPhong)
                    .HasName("PK__PhongBen__20BD5E5BA6869730");

                entity.ToTable("PhongBenh");

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LoaiPhong).HasMaxLength(50);

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SoGiuong).HasDefaultValueSql("((0))");

                entity.Property(e => e.SoGiuongTrong).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenPhong).HasMaxLength(50);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MaKhoaNavigation)
                    .WithMany(p => p.PhongBenhs)
                    .HasForeignKey(d => d.MaKhoa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PhongBenh__MaKho__4E88ABD4");
            });

            modelBuilder.Entity<TheoDoiBenhNhan>(entity =>
            {
                entity.HasKey(e => e.MaTheoDoi)
                    .HasName("PK__TheoDoiB__3156C0791CABA09E");

                entity.ToTable("TheoDoiBenhNhan");

                entity.HasIndex(e => e.MaBenhNhan, "IX_TheoDoiBenhNhan_MaBenhNhan");

                entity.HasIndex(e => e.NgayTheoDoi, "IX_TheoDoiBenhNhan_NgayTheoDoi");

                entity.Property(e => e.MaTheoDoi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CanNang).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ChiSoKhac).HasMaxLength(200);

                entity.Property(e => e.ChieuCao).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.DanhGia).HasMaxLength(100);

                entity.Property(e => e.GhiChu).HasMaxLength(300);

                entity.Property(e => e.HuyetAp)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaYTa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaYTa")
                    .IsFixedLength();

                entity.Property(e => e.NgayTheoDoi)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NhietDo).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.NhietDoHoiTho).HasColumnName("NhietDo_HoiTho");

                entity.HasOne(d => d.MaBenhNhanNavigation)
                    .WithMany(p => p.TheoDoiBenhNhans)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TheoDoiBe__MaBen__3B40CD36");

                entity.HasOne(d => d.MaYtaNavigation)
                    .WithMany(p => p.TheoDoiBenhNhans)
                    .HasForeignKey(d => d.MaYTa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TheoDoiBe__MaYTa__3C34F16F");
            });

            modelBuilder.Entity<ThietBi>(entity =>
            {
                entity.HasKey(e => e.MaThietBi)
                    .HasName("PK__ThietBi__8AEC71F70D3DE1F5");

                entity.ToTable("ThietBi");

                entity.Property(e => e.MaThietBi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.GhiChu).HasMaxLength(300);

                entity.Property(e => e.HanBaoHanh).HasColumnType("date");

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayKiemKe).HasColumnType("date");

                entity.Property(e => e.NgayMua).HasColumnType("date");

                entity.Property(e => e.NguoiQuanLy)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PhongBan).HasMaxLength(50);

                entity.Property(e => e.TenThietBi).HasMaxLength(100);

                entity.Property(e => e.TinhTrang)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'Hoạt động')");

                entity.HasOne(d => d.MaPhongNavigation)
                    .WithMany(p => p.ThietBis)
                    .HasForeignKey(d => d.MaPhong)
                    .HasConstraintName("FK__ThietBi__MaPhong__282DF8C2");

                entity.HasOne(d => d.NguoiQuanLyNavigation)
                    .WithMany(p => p.ThietBis)
                    .HasForeignKey(d => d.NguoiQuanLy)
                    .HasConstraintName("FK__ThietBi__NguoiQu__2B0A656D");
            });

            modelBuilder.Entity<Thuoc>(entity =>
            {
                entity.HasKey(e => e.MaThuoc)
                    .HasName("PK__Thuoc__4BB1F620FB5CEA0F");

                entity.ToTable("Thuoc");

                entity.HasIndex(e => e.TenThuoc, "IX_Thuoc_TenThuoc");

                entity.Property(e => e.MaThuoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ChongChiDinh).HasMaxLength(500);

                entity.Property(e => e.DonGia)
                    .HasColumnType("decimal(15, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DonViTinh).HasMaxLength(20);

                entity.Property(e => e.HanSuDung).HasColumnType("date");

                entity.Property(e => e.LoaiThuoc).HasMaxLength(50);

                entity.Property(e => e.NhaSanXuat).HasMaxLength(100);

                entity.Property(e => e.NuocSanXuat).HasMaxLength(50);

                entity.Property(e => e.SoLuongTon).HasDefaultValueSql("((0))");

                entity.Property(e => e.TenThuoc).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UserLock>(entity =>
            {
                entity.HasKey(e => e.LockKey)
                    .HasName("PK__UserLock__A77F8126FCD95360");

                entity.Property(e => e.LockKey).HasMaxLength(255);

                entity.Property(e => e.LockTime).HasColumnType("datetime");

                entity.Property(e => e.LockedBy).HasMaxLength(100);

                entity.Property(e => e.RecordId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RecordID")
                    .IsFixedLength();

                entity.Property(e => e.TableName).HasMaxLength(100);

                entity.Property(e => e.TimeoutMinutes).HasDefaultValueSql("((5))");
            });

            modelBuilder.Entity<VwBaoCaoBenhTheoKhoa>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_BaoCaoBenhTheoKhoa");

                entity.Property(e => e.ChuanDoan).HasMaxLength(200);

                entity.Property(e => e.DenNgay).HasColumnType("date");

                entity.Property(e => e.MaKhoa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TenKhoa).HasMaxLength(100);

                entity.Property(e => e.TuNgay).HasColumnType("date");
            });

            modelBuilder.Entity<VwBenhNhanCanTheoDoi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_BenhNhanCanTheoDoi");

                entity.Property(e => e.ChuanDoan).HasMaxLength(200);

                entity.Property(e => e.HoTenBenhNhan).HasMaxLength(100);

                entity.Property(e => e.HuyetAp)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaBenhNhan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayNhapVien).HasColumnType("date");

                entity.Property(e => e.NhietDo).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.TenBacSi).HasMaxLength(100);

                entity.Property(e => e.TenKhoa).HasMaxLength(100);

                entity.Property(e => e.TenPhong).HasMaxLength(50);

                entity.Property(e => e.TinhTrang).HasMaxLength(100);
            });

            modelBuilder.Entity<VwDoanhThuDichVu>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_DoanhThuDichVu");

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.MaDichVu)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TenDichVu).HasMaxLength(100);

                entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 2)");
            });

            modelBuilder.Entity<VwDoanhThuTheoThang>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_DoanhThuTheoThang");

                entity.Property(e => e.DoanhThuTrungBinh).HasColumnType("decimal(38, 6)");

                entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 2)");
            });

            modelBuilder.Entity<VwLichHenHomNay>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_LichHenHomNay");

                entity.Property(e => e.HoTenBenhNhan).HasMaxLength(100);

                entity.Property(e => e.LyDoKham).HasMaxLength(200);

                entity.Property(e => e.MaLichHen)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayHen).HasColumnType("datetime");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TenBacSi).HasMaxLength(100);

                entity.Property(e => e.TenKhoa).HasMaxLength(100);

                entity.Property(e => e.TrangThai).HasMaxLength(20);
            });

            modelBuilder.Entity<VwLichLamViecNhanVien>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_LichLamViecNhanVien");

                entity.Property(e => e.CaLamViec).HasMaxLength(20);

                entity.Property(e => e.ChucVu).HasMaxLength(50);

                entity.Property(e => e.GhiChu).HasMaxLength(200);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.LoaiNhanVien).HasMaxLength(10);

                entity.Property(e => e.MaLich)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayLamViec).HasColumnType("date");

                entity.Property(e => e.TenKhoa).HasMaxLength(100);

                entity.Property(e => e.TenPhong).HasMaxLength(50);
            });

            modelBuilder.Entity<VwThongKeBenhNhan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_ThongKeBenhNhan");

                entity.Property(e => e.TenKhoa).HasMaxLength(100);
            });

            modelBuilder.Entity<VwThongKeHoatDong>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_ThongKeHoatDong");

                entity.Property(e => e.DoanhThuHomNay).HasColumnType("decimal(38, 2)");

                entity.Property(e => e.TongCongNo).HasColumnType("decimal(38, 2)");
            });

            modelBuilder.Entity<VwThongKeThietBi>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_ThongKeThietBi");

                entity.Property(e => e.HanBaoHanh).HasColumnType("date");

                entity.Property(e => e.MaThietBi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayMua).HasColumnType("date");

                entity.Property(e => e.TenKhoa).HasMaxLength(100);

                entity.Property(e => e.TenPhong).HasMaxLength(50);

                entity.Property(e => e.TenThietBi).HasMaxLength(100);

                entity.Property(e => e.TinhTrang).HasMaxLength(50);

                entity.Property(e => e.TrangThaiChiTiet).HasMaxLength(19);
            });

            modelBuilder.Entity<VwThongKeThuoc>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_ThongKeThuoc");

                entity.Property(e => e.DoanhThuThuoc).HasColumnType("decimal(38, 2)");

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.DonViTinh).HasMaxLength(20);

                entity.Property(e => e.HanSuDung).HasColumnType("date");

                entity.Property(e => e.MaThuoc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TenThuoc).HasMaxLength(100);

                entity.Property(e => e.TinhTrang).HasMaxLength(11);
            });

            modelBuilder.Entity<VwThuocSapHetHan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_ThuocSapHetHan");

                entity.Property(e => e.HanSuDung).HasColumnType("date");

                entity.Property(e => e.TenThuoc).HasMaxLength(100);
            });

            modelBuilder.Entity<YTa>(entity =>
            {
                entity.HasKey(e => e.MaYTa)
                    .HasName("PK__YTa__2096BFE262DA2513");

                entity.ToTable("YTa");

                entity.HasIndex(e => e.MaPhong, "IX_YTa_MaPhong");

                entity.HasIndex(e => e.Cccd, "UQ__YTa__A955A0AACC603569")
                    .IsUnique();

                entity.Property(e => e.MaYTa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaYTa")
                    .IsFixedLength();

                entity.Property(e => e.Cccd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CCCD");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.KinhNghiem).HasDefaultValueSql("((0))");

                entity.Property(e => e.MaPhong)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgayVaoLam)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SoDienThoai)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TenYTa)
                    .HasMaxLength(100)
                    .HasColumnName("TenYTa");

                entity.Property(e => e.TrangThai).HasDefaultValueSql("((1))");

                entity.Property(e => e.TrinhDo).HasMaxLength(50);

                entity.HasOne(d => d.MaPhongNavigation)
                    .WithMany(p => p.Yta)
                    .HasForeignKey(d => d.MaPhong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__YTa__MaPhong__656C112C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
