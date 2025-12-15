using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using App.ViewModels;
using System.Security.Claims; // Cần thêm cái này để lấy User hiện tại

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
        public IActionResult Create()
        {
            TempData["Error"] = "Vui lòng Thêm Bác sĩ từ trang 'Thêm Nhân viên mới' và chọn Loại Nhân viên là 'Bác sĩ'.";
            return RedirectToAction("Create", "NhanVien");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVienBacSiViewModel vm)
        {
            ModelState.AddModelError("", "Lỗi Logic: Không được phép thêm Bác sĩ từ trang này. Hãy dùng NhanVien/Create.");
            LoadKhoaList(vm.MaKhoa);
            return View(vm);
        }

        // =========================================================
        // SỬA ACTION EDIT (GET): THÊM CODE LOCKING (sp_AcquireLock)
        // =========================================================
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            string trimmedId = id.Trim();

            // --- BƯỚC 1: CỐ GẮNG LẤY KHÓA (ACQUIRE LOCK) ---
            // Lấy tên user đang đăng nhập (Email hoặc Username)
            string currentUser = User.Identity?.Name ?? "Unknown User";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("sp_AcquireLock", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TableName", "BacSi"); // Tên bảng cần khóa
                        cmd.Parameters.AddWithValue("@RecordID", trimmedId); // Mã bản ghi (MaBacSi)
                        cmd.Parameters.AddWithValue("@UserName", currentUser); // Người đang muốn sửa

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int success = reader.GetInt32(reader.GetOrdinal("Success"));
                                string message = reader.GetString(reader.GetOrdinal("Message"));

                                // Nếu Success == 0 nghĩa là người khác đang sửa -> Chặn lại
                                if (success == 0)
                                {
                                    TempData["Error"] = message; // Hiện thông báo: "Bảng đang được sử dụng bởi..."
                                    return RedirectToAction(nameof(Index)); // Đẩy về trang danh sách
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi kết nối Lock thì thôi, vẫn cho sửa nhưng log lỗi (hoặc chặn luôn tùy bạn)
                Console.WriteLine("Lỗi AcquireLock: " + ex.Message);
            }
            // --- KẾT THÚC BƯỚC 1 ---


            // --- BƯỚC 2: LOAD DỮ LIỆU NHƯ CŨ ---
            var bacSi = await _context.BacSis.FirstOrDefaultAsync(b => b.MaBacSi.Trim() == trimmedId);
            if (bacSi == null) return NotFound();

            var nhanVien = await _context.NhanViens.FirstOrDefaultAsync(n => n.Cccd == bacSi.Cccd);
            if (nhanVien == null) return NotFound();

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

            LoadKhoaList(vm.MaKhoa);
            return View(vm);
        }

        // =============================================================
        // SỬA ACTION EDIT (POST): THÊM CODE NHẢ KHÓA (sp_ReleaseLock)
        // =============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NhanVienBacSiViewModel vm)
        {
            if (id.Trim() != vm.MaNhanVien.Trim()) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    using (var conn = new SqlConnection(_connectionString))
                    {
                        await conn.OpenAsync();

                        // 1. Cập nhật dữ liệu (Logic cũ)
                        using (var cmd = new SqlCommand("sp_CapNhatBacSi", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
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
                            cmd.Parameters.AddWithValue("@MaKhoa", vm.MaKhoa);
                            cmd.Parameters.AddWithValue("@ChuyenKhoa", (object)vm.ChuyenKhoa ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@HocVi", (object)vm.HocVi ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@BangCap", (object)vm.BangCap ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@KinhNghiem", (object)vm.KinhNghiem ?? DBNull.Value);

                            await cmd.ExecuteNonQueryAsync();
                        }

                        // --- 2. NHẢ KHÓA (RELEASE LOCK) SAU KHI LƯU XONG ---
                        string currentUser = User.Identity?.Name ?? "Unknown User";
                        using (var cmdLock = new SqlCommand("sp_ReleaseLock", conn))
                        {
                            cmdLock.CommandType = CommandType.StoredProcedure;
                            cmdLock.Parameters.AddWithValue("@TableName", "BacSi");
                            cmdLock.Parameters.AddWithValue("@RecordID", id.Trim());
                            cmdLock.Parameters.AddWithValue("@UserName", currentUser);

                            await cmdLock.ExecuteNonQueryAsync();
                        }
                        // --- HẾT PHẦN NHẢ KHÓA ---
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

        // =============================================================
        // ACTION MỚI: NHẢ KHÓA KHI BẤM "HỦY" (CANCEL) HOẶC "BACK"
        // (Bạn cần thêm nút gọi action này trong View Edit.cshtml nếu muốn kỹ hơn)
        // =============================================================
        public async Task<IActionResult> CancelEdit(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction(nameof(Index));

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string currentUser = User.Identity?.Name ?? "Unknown User";
                    using (var cmd = new SqlCommand("sp_ReleaseLock", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TableName", "BacSi");
                        cmd.Parameters.AddWithValue("@RecordID", id.Trim());
                        cmd.Parameters.AddWithValue("@UserName", currentUser);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch { } // Lỗi nhả khóa khi hủy thì bỏ qua

            return RedirectToAction(nameof(Index));
        }

        // ===================================================================
        // TOGGLE STATUS: Đổi trạng thái Bác sĩ (Không ảnh hưởng Nhân viên)
        // ===================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    // Gọi SP mới: sp_ToggleTrangThaiBacSi
                    using (var cmd = new SqlCommand("sp_ToggleTrangThaiBacSi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaBacSi", id.Trim()); // Nhớ Trim() để tránh lỗi CHAR(10)

                        // Đọc thông báo trả về
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                string msg = reader.GetString(reader.GetOrdinal("Message"));
                                TempData["Success"] = msg;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi đổi trạng thái: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}