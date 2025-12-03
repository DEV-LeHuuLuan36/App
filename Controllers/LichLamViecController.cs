using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    // Module này cho phép DevLead/Admin xếp lịch
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class LichLamViecController : Controller
    {
        private readonly QLBenhVienContext _context;

        public LichLamViecController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Hàm helper để tải danh sách Nhân viên (Bác sĩ, Y tá)
        private void LoadNhanVienList(string? selectedValue = null) // Thêm selectedValue
        {
            // === SỬA LỖI LOGIC 1.1 ===
            // 1. Lọc đúng NhanVien (CHA) với TrangThai = 1
            // 2. Lọc đúng LoaiNhanVien CÓ DẤU (theo CSDL)
            var nhanViens = _context.NhanViens
                .Where(n => n.TrangThai == true && (n.LoaiNhanVien == "Bác sĩ" || n.LoaiNhanVien == "Y tá"))
                .Select(n => new {
                    ID = n.MaNhanVien,
                    Ten = n.HoTen + " (" + n.LoaiNhanVien + ")", // CSDL đã lưu có dấu
                    Loai = n.LoaiNhanVien // CSDL đã lưu có dấu
                })
                .OrderBy(n => n.Ten)
                .ToList();

            // Dùng SelectList có OptGroup (Nhóm)
            ViewData["MaNhanVien"] = new SelectList(nhanViens, "ID", "Ten", selectedValue, "Loai");
        }

        // GET: LichLamViec
        public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay, string? maNhanVien)
        {
            var query = _context.LichLamViecs
                .Include(l => l.MaNhanVienNavigation)
                .OrderByDescending(l => l.NgayLamViec)
                .AsQueryable();

            if (tuNgay.HasValue)
            {
                query = query.Where(l => l.NgayLamViec >= tuNgay.Value);
                ViewData["TuNgay"] = tuNgay.Value.ToString("yyyy-MM-dd");
            }
            if (denNgay.HasValue)
            {
                query = query.Where(l => l.NgayLamViec <= denNgay.Value);
                ViewData["DenNgay"] = denNgay.Value.ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(maNhanVien))
            {
                // SỬA LỖI 404 (CHAR(10)): Phải Trim() khi so sánh
                query = query.Where(l => l.MaNhanVien.Trim() == maNhanVien.Trim());
                ViewData["MaNhanVien"] = maNhanVien;
            }

            LoadNhanVienList(maNhanVien); // Truyền giá trị đã chọn
            return View(await query.ToListAsync());
        }

        // GET: LichLamViec/Create
        public IActionResult Create()
        {
            LoadNhanVienList();
            return View();
        }

        // POST: LichLamViec/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLich,MaNhanVien,LoaiNhanVien,NgayLamViec,CaLamViec,GhiChu,TrangThai")] LichLamViec lichLamViec)
        {
            ModelState.Remove("MaNhanVienNavigation");

            // SỬA LỖI LOGIC: Tự động gán LoaiNhanVien (không dấu)
            // theo CHECK constraint của bảng LichLamViec
            var nhanVien = await _context.NhanViens.FindAsync(lichLamViec.MaNhanVien);
            if (nhanVien != null)
            {
                if (nhanVien.LoaiNhanVien == "Bác sĩ")
                    lichLamViec.LoaiNhanVien = "BacSi";
                else if (nhanVien.LoaiNhanVien == "Y tá")
                    lichLamViec.LoaiNhanVien = "YTa";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(lichLamViec);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo lịch làm việc thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    string errorMessage = ex.Message;
                    if (ex.InnerException != null)
                        errorMessage = ex.InnerException.Message;

                    if (errorMessage.Contains("PRIMARY KEY constraint"))
                    {
                        ModelState.AddModelError("MaLich", $"Mã lịch '{lichLamViec.MaLich}' đã tồn tại.");
                    }
                    else if (errorMessage.Contains("FOREIGN KEY constraint"))
                    {
                        ModelState.AddModelError("MaNhanVien", "Mã nhân viên không tồn tại trong CSDL.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi CSDL: " + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            LoadNhanVienList(lichLamViec.MaNhanVien);
            return View(lichLamViec);
        }

        // GET: LichLamViec/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            // SỬA LỖI 404 (CHAR(10))
            var lichLamViec = await _context.LichLamViecs
                .FirstOrDefaultAsync(l => l.MaLich.Trim() == id.Trim());

            if (lichLamViec == null) return NotFound();

            LoadNhanVienList(lichLamViec.MaNhanVien);
            return View(lichLamViec);
        }

        // POST: LichLamViec/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLich,MaNhanVien,LoaiNhanVien,NgayLamViec,CaLamViec,GhiChu,TrangThai")] LichLamViec lichLamViec)
        {
            if (id.Trim() != lichLamViec.MaLich.Trim()) return NotFound(); // SỬA LỖI 404

            ModelState.Remove("MaNhanVienNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lichLamViec);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật lịch làm việc thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            LoadNhanVienList(lichLamViec.MaNhanVien);
            return View(lichLamViec);
        }

        // GET: LichLamViec/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            // SỬA LỖI 404 (CHAR(10))
            var lichLamViec = await _context.LichLamViecs
                .Include(l => l.MaNhanVienNavigation)
                .FirstOrDefaultAsync(m => m.MaLich.Trim() == id.Trim());

            if (lichLamViec == null) return NotFound();
            return View(lichLamViec);
        }

        // POST: LichLamViec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                // SỬA LỖI 404 (CHAR(10))
                var lichLamViec = await _context.LichLamViecs
                    .FirstOrDefaultAsync(l => l.MaLich.Trim() == id.Trim());

                if (lichLamViec != null)
                {
                    _context.LichLamViecs.Remove(lichLamViec);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa lịch làm việc thành công.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}