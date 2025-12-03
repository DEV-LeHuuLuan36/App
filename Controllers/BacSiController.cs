using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using App.ViewModels;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class BacSiController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly string _connectionString;

        public BacSiController(QLBenhVienContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private void LoadKhoaList(string? selectedValue = null)
        {
            ViewData["MaKhoa"] = new SelectList(_context.Khoas.Where(k => k.TrangThai == true), "MaKhoa", "TenKhoa", selectedValue);
        }

        public async Task<IActionResult> Index()
        {
            var bacSis = _context.BacSis
                .Include(b => b.MaKhoaNavigation)
                .OrderBy(b => b.TenBacSi);

            return View(await bacSis.ToListAsync());
        }

        // GET: BacSi/Create
        // SỬA LỖI LOGIC 1.1: Chuyển hướng đến NhanVien/Create
        [Obsolete("Dùng NhanVien/Create với LoaiNhanVien = 'Bác sĩ'")]
        public IActionResult Create()
        {
            TempData["Error"] = "Lỗi Logic: Vui lòng Thêm Bác sĩ từ trang 'Thêm Nhân viên mới' và chọn Loại Nhân viên là 'Bác sĩ'.";
            return RedirectToAction("Create", "NhanVien");
        }

        // POST: BacSi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete("Dùng NhanVien/Create với LoaiNhanVien = 'Bác sĩ'")]
        public async Task<IActionResult> Create(NhanVienBacSiViewModel vm)
        {
            // Hàm này không nên được gọi
            ModelState.AddModelError("", "Lỗi Logic: Không được phép thêm Bác sĩ từ trang này. Hãy dùng NhanVien/Create.");
            LoadKhoaList(vm.MaKhoa);
            return View(vm);
        }


        //
        // === SỬA LỖI 404 VÀ LỖI InvalidOperationException TẠI ĐÂY ===
        //
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            // --- SỬA LỖI 404 (DO CHAR(10)) ---
            // Dùng Where và Trim() để tìm kiếm
            string trimmedId = id.Trim();

            // Tìm Bác sĩ (bảng CON)
            var bacSi = await _context.BacSis
                .FirstOrDefaultAsync(b => b.MaBacSi.Trim() == trimmedId);

            if (bacSi == null)
            {
                TempData["Error"] = $"Không tìm thấy Bác sĩ với ID = {id}.";
                return RedirectToAction(nameof(Index));
            }

            // Dùng Cccd (Khóa duy nhất) để tìm NhanVien (bảng CHA) tương ứng
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(n => n.Cccd == bacSi.Cccd);

            if (nhanVien == null)
            {
                TempData["Error"] = $"Lỗi Logic Kế thừa: Không tìm thấy NhanVien (CHA) tương ứng với BacSi (CON) ID = {id}. (Cccd: {bacSi.Cccd})";
                return RedirectToAction(nameof(Index));
            }

            // --- SỬA LỖI InvalidOperationException ---
            // Phải tạo ViewModel (vm) và trả về (return View(vm))
            var vm = new NhanVienBacSiViewModel
            {
                MaNhanVien = nhanVien.MaNhanVien,
                HoTen = nhanVien.HoTen,
                NgaySinh = nhanVien.NgaySinh,
                GioiTinh = nhanVien.GioiTinh,
                SoDienThoai = nhanVien.SoDienThoai,
                Email = nhanVien.Email,
                DiaChi = nhanVien.DiaChi,
                CCCD = nhanVien.Cccd,
                TrinhDo = nhanVien.TrinhDo,
                NgayVaoLam = nhanVien.NgayVaoLam,
                TrangThai = nhanVien.TrangThai ?? true,

                MaKhoa = bacSi.MaKhoa,
                ChuyenKhoa = bacSi.ChuyenKhoa,
                HocVi = bacSi.HocVi,
                BangCap = bacSi.BangCap,
                KinhNghiem = bacSi.KinhNghiem
            };
            // --- HẾT SỬA ---

            LoadKhoaList(vm.MaKhoa);
            return View(vm); // << Trả về ViewModel (vm)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NhanVienBacSiViewModel vm)
        {
            // --- SỬA LỖI 404 (DO CHAR(10)) ---
            if (id.Trim() != vm.MaNhanVien.Trim()) return NotFound();
            // --- HẾT SỬA ---

            // Sửa lỗi validation 'NhanVienBacSiViewModel' (namespace)
            // (ViewModel của bạn nằm trong App.Models, tôi đã sửa using ở trên)
            if (ModelState.IsValid)
            {
                try
                {
                    // Dùng SP (Vì SP đã sửa lỗi Logic Kế thừa)
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        await conn.OpenAsync();
                        using (var cmd = new SqlCommand("sp_CapNhatBacSi", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // 1. Thông tin NhanVien (chung)
                            cmd.Parameters.AddWithValue("@MaNhanVien", vm.MaNhanVien);
                            cmd.Parameters.AddWithValue("@HoTen", vm.HoTen);
                            cmd.Parameters.AddWithValue("@NgaySinh", (object)vm.NgaySinh ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@GioiTinh", vm.GioiTinh);
                            cmd.Parameters.AddWithValue("@SoDienThoai", (object)vm.SoDienThoai ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Email", (object)vm.Email ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@DiaChi", (object)vm.DiaChi ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Cccd", (object)vm.CCCD ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@TrinhDo", (object)vm.TrinhDo ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@NgayVaoLam", (object)vm.NgayVaoLam ?? DBNull.Value);

                            // 2. Thông tin BacSi (riêng)
                            cmd.Parameters.AddWithValue("@MaKhoa", vm.MaKhoa);
                            cmd.Parameters.AddWithValue("@ChuyenKhoa", (object)vm.ChuyenKhoa ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@HocVi", (object)vm.HocVi ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@BangCap", (object)vm.BangCap ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@KinhNghiem", (object)vm.KinhNghiem ?? DBNull.Value);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    TempData["Success"] = "Cập nhật bác sĩ thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex)
                {
                    ModelState.AddModelError("", "Lỗi CSDL: " + ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            LoadKhoaList(vm.MaKhoa);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();
            try
            {
                // Dùng SP (Vì SP đã sửa lỗi Logic Kế thừa)
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_XoaBacSi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaBacSi", id.Trim()); // SỬA LỖI 404
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                TempData["Success"] = "Cập nhật trạng thái Bác sĩ thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}