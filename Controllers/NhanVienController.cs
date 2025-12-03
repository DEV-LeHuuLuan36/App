using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.Data.SqlClient;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class NhanVienController : Controller
    {
        private readonly QLBenhVienContext _context;
        // Lấy chuỗi kết nối (Connection String) từ appsettings.json
        private readonly string _connectionString;


        public NhanVienController(QLBenhVienContext context, IConfiguration configuration)
        {
            _context = context;
            // Thêm IConfiguration vào constructor để lấy chuỗi kết nối
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ĐÃ SỬA: Thêm selectedMaKhoa
        private void LoadKhoaList(string? selectedMaKhoa = null)
        {
            ViewData["MaKhoa"] = new SelectList(_context.Khoas.Where(k => k.TrangThai == true), "MaKhoa", "TenKhoa", selectedMaKhoa);
        }

        public async Task<IActionResult> Index(string chucVu)
        {
            var query = _context.NhanViens
                .Include(n => n.MaKhoaNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(chucVu))
            {
                query = query.Where(n => n.ChucVu == chucVu);
                ViewData["ChucVuFilter"] = chucVu;
            }

            return View(await query.OrderBy(n => n.HoTen).ToListAsync());
        }

        public IActionResult Create()
        {
            LoadKhoaList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // ĐÃ SỬA: Bind "Cccd" (từ "Cccd" trong file .sql)
        public async Task<IActionResult> Create([Bind("MaNhanVien,HoTen,NgaySinh,GioiTinh,SoDienThoai,Email,DiaChi,Cccd,ChucVu,TrinhDo,NgayVaoLam,MaKhoa,LoaiNhanVien,TrangThai")] NhanVien nhanVien)
        {
            ModelState.Remove("MaKhoaNavigation");
            nhanVien.TrangThai = true; // Mặc định là đang làm việc

            if (ModelState.IsValid)
            {
                // ================================================================
                // === SỬA LỖI LOGIC KẾ THỪA (LỖI 1.1) ===
                // ================================================================

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Bước 1: Luôn INSERT vào bảng NhanVien (Bảng CHA)
                        _context.Add(nhanVien);

                        // Bước 2: Kiểm tra LoaiNhanVien và INSERT vào bảng CON (nếu cần)
                        if (nhanVien.LoaiNhanVien == "Bác sĩ")
                        {
                            BacSi bacSi = new BacSi
                            {
                                MaBacSi = nhanVien.MaNhanVien, // << DÙNG CHUNG ID
                                TenBacSi = nhanVien.HoTen,
                                MaKhoa = nhanVien.MaKhoa,
                                SoDienThoai = nhanVien.SoDienThoai,
                                Email = nhanVien.Email,
                                Cccd = nhanVien.Cccd, // Dùng Cccd từ NhanVien
                                NgayVaoLam = nhanVien.NgayVaoLam,
                                // TrinhDo (Bảng BacSi không có)
                                TrangThai = true,
                                ChuyenKhoa = null,
                                HocVi = nhanVien.TrinhDo, // << GÁN TẠM TrinhDo vào HocVi
                                BangCap = null,
                                KinhNghiem = 0
                            };
                            _context.BacSis.Add(bacSi);
                        }
                        else if (nhanVien.LoaiNhanVien == "Y tá")
                        {
                            YTa yTa = new YTa
                            {
                                MaYTa = nhanVien.MaNhanVien, // << DÙNG CHUNG ID
                                TenYTa = nhanVien.HoTen,
                                MaPhong = "P101", // << GÁN TẠM (BẠN PHẢI SỬA FORM)
                                TrinhDo = nhanVien.TrinhDo,
                                SoDienThoai = nhanVien.SoDienThoai,
                                Email = nhanVien.Email,
                                Cccd = nhanVien.Cccd,
                                NgayVaoLam = nhanVien.NgayVaoLam,
                                TrangThai = true,
                                KinhNghiem = 0
                            };
                            _context.Yta.Add(yTa);
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        TempData["Success"] = "Thêm nhân viên thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException ex) // Bắt lỗi CSDL
                    {
                        await transaction.RollbackAsync(); // Hoàn tác

                        string errorMessage = ex.Message;
                        if (ex.InnerException != null)
                        {
                            errorMessage = ex.InnerException.Message;
                        }

                        if (errorMessage.Contains("PRIMARY KEY"))
                        {
                            ModelState.AddModelError("MaNhanVien", $"Mã nhân viên '{nhanVien.MaNhanVien}' đã tồn tại.");
                        }
                        else if (errorMessage.Contains("UNIQUE KEY") && (errorMessage.Contains("Cccd") || errorMessage.Contains("Cccd")))
                        {
                            ModelState.AddModelError("Cccd", $"Cccd '{nhanVien.Cccd}' đã được sử dụng.");
                        }
                        else if (errorMessage.Contains("FOREIGN KEY") && errorMessage.Contains("MaKhoa"))
                        {
                            ModelState.AddModelError("MaKhoa", "Khoa bạn chọn không hợp lệ.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Lỗi CSDL: " + errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                    }
                }
                // ================================================================
                // === KẾT THÚC SỬA LỖI LOGIC 1.1 ===
                // ================================================================
            }
            LoadKhoaList(nhanVien.MaKhoa);
            return View(nhanVien);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            // --- SỬA LỖI 404 (DO CHAR(10)) ---
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(n => n.MaNhanVien.Trim() == id.Trim());
            // --- HẾT SỬA ---

            if (nhanVien == null) return NotFound();

            ViewData["MaKhoa"] = new SelectList(_context.Khoas.Where(k => k.TrangThai == true), "MaKhoa", "TenKhoa", nhanVien.MaKhoa);
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // ĐÃ SỬA: Bind "Cccd"
        public async Task<IActionResult> Edit(string id, [Bind("MaNhanVien,HoTen,NgaySinh,GioiTinh,SoDienThoai,Email,DiaChi,Cccd,ChucVu,TrinhDo,NgayVaoLam,MaKhoa,LoaiNhanVien,TrangThai")] NhanVien nhanVien)
        {
            if (id.Trim() != nhanVien.MaNhanVien.Trim()) return NotFound(); // SỬA LỖI 404

            ModelState.Remove("MaKhoaNavigation");

            if (ModelState.IsValid)
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Bước 1: Cập nhật NhanVien (CHA)
                        _context.Update(nhanVien);

                        // Bước 2: Cập nhật Bác sĩ (CON) nếu có
                        if (nhanVien.LoaiNhanVien == "Bác sĩ")
                        {
                            // SỬA LỖI 404: Dùng Trim()
                            var bacSi = await _context.BacSis.FirstOrDefaultAsync(b => b.MaBacSi.Trim() == id.Trim());
                            if (bacSi != null)
                            {
                                // Cập nhật các trường chung
                                bacSi.TenBacSi = nhanVien.HoTen;
                                bacSi.MaKhoa = nhanVien.MaKhoa;
                                bacSi.SoDienThoai = nhanVien.SoDienThoai;
                                bacSi.Email = nhanVien.Email;
                                bacSi.Cccd = nhanVien.Cccd;

                                // --- SỬA LỖI 'BacSi' does not contain 'TrinhDo' ---
                                // Bảng BacSi không có TrinhDo, nhưng có HocVi
                                // Tạm thời gán TrinhDo của NhanVien vào HocVi của BacSi
                                bacSi.HocVi = nhanVien.TrinhDo;
                                // --- HẾT SỬA ---

                                bacSi.NgayVaoLam = nhanVien.NgayVaoLam;
                                bacSi.TrangThai = nhanVien.TrangThai;
                                _context.Update(bacSi);
                            }
                        }
                        // Bước 3: Cập nhật Y tá (CON) nếu có
                        else if (nhanVien.LoaiNhanVien == "Y tá")
                        {
                            // SỬA LỖI 404: Dùng Trim()
                            var yTa = await _context.Yta.FirstOrDefaultAsync(y => y.MaYTa.Trim() == id.Trim());
                            if (yTa != null)
                            {
                                // Cập nhật các trường chung
                                yTa.TenYTa = nhanVien.HoTen;
                                yTa.TrinhDo = nhanVien.TrinhDo; // Bảng YTa có TrinhDo
                                yTa.SoDienThoai = nhanVien.SoDienThoai;
                                yTa.Email = nhanVien.Email;
                                yTa.Cccd = nhanVien.Cccd;
                                yTa.NgayVaoLam = nhanVien.NgayVaoLam;
                                yTa.TrangThai = nhanVien.TrangThai;
                                // yTa.MaPhong (Form Edit NhanVien không có, giữ nguyên MaPhong cũ)
                                _context.Update(yTa);
                            }
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        TempData["Success"] = "Cập nhật nhân viên thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                    }
                }
            }
            ViewData["MaKhoa"] = new SelectList(_context.Khoas.Where(k => k.TrangThai == true), "MaKhoa", "TenKhoa", nhanVien.MaKhoa);
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            // SỬA LỖI 404
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(n => n.MaNhanVien.Trim() == id.Trim());

            if (nhanVien == null) return NotFound();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Lật ngược trạng thái
                    nhanVien.TrangThai = !nhanVien.TrangThai;
                    _context.Update(nhanVien);

                    // Cập nhật Bác sĩ (CON) nếu có
                    if (nhanVien.LoaiNhanVien == "Bác sĩ")
                    {
                        var bacSi = await _context.BacSis.FindAsync(nhanVien.MaNhanVien); // Dùng ID đã Trim
                        if (bacSi != null)
                        {
                            bacSi.TrangThai = nhanVien.TrangThai;
                            _context.Update(bacSi);
                        }
                    }
                    // Cập nhật Y tá (CON) nếu có
                    else if (nhanVien.LoaiNhanVien == "Y tá")
                    {
                        var yTa = await _context.Yta.FindAsync(nhanVien.MaNhanVien); // Dùng ID đã Trim
                        if (yTa != null)
                        {
                            yTa.TrangThai = nhanVien.TrangThai;
                            _context.Update(yTa);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    TempData["Success"] = "Cập nhật trạng thái Nhân viên thành công.";
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}