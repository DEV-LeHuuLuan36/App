using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class DichVuController : Controller
    {
        private readonly QLBenhVienContext _context;

        public DichVuController(QLBenhVienContext context)
        {
            _context = context;
        }

        // GET: DichVu
        public async Task<IActionResult> Index()
        {
            // SP 'sp_LayDanhSachDichVu' của bạn chỉ lấy TrangThai = 1.
            // Để Admin có thể quản lý (tắt/bật), chúng ta sẽ dùng EF Core 
            // để lấy TẤT CẢ dịch vụ, bao gồm cả dịch vụ đã "Xóa" (TrangThai = 0).
            var dichVus = _context.DichVus.OrderBy(d => d.TenDichVu);
            return View(await dichVus.ToListAsync());
        }

        // GET: DichVu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DichVu/Create (Gọi sp_ThemDichVu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDichVu,TenDichVu,DonGia,MoTa")] DichVu dichVu)
        {
            // Gán TrangThai = 1 (Active) khi tạo mới
            dichVu.TrangThai = true;
            ModelState.Remove("ChiTietHoaDons"); // Bỏ qua thuộc tính navigation

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi SP "sp_ThemDichVu"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_ThemDichVu
                            @MaDichVu = {dichVu.MaDichVu},
                            @TenDichVu = {dichVu.TenDichVu},
                            @DonGia = {dichVu.DonGia},
                            @MoTa = {dichVu.MoTa}
                    """);

                    TempData["Success"] = "Thêm dịch vụ thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            return View(dichVu);
        }

        // GET: DichVu/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null) return NotFound();
            return View(dichVu);
        }

        // POST: DichVu/Edit/5 (Gọi sp_CapNhatDichVu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaDichVu,TenDichVu,DonGia,MoTa,TrangThai")] DichVu dichVu)
        {
            if (id != dichVu.MaDichVu) return NotFound();

            ModelState.Remove("ChiTietHoaDons");

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Gọi SP "sp_CapNhatDichVu" (cập nhật thông tin chính)
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_CapNhatDichVu
                            @MaDichVu = {dichVu.MaDichVu},
                            @TenDichVu = {dichVu.TenDichVu},
                            @DonGia = {dichVu.DonGia},
                            @MoTa = {dichVu.MoTa}
                    """);

                    // 2. SP của bạn không cập nhật 'TrangThai' (Active/Inactive)
                    // Chúng ta sẽ dùng EF Core để cập nhật riêng trường đó.
                    _context.Update(dichVu);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật dịch vụ thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            return View(dichVu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            var dichVu = await _context.DichVus.FindAsync(id);
            if (dichVu == null) return NotFound();

            try
            {
                dichVu.TrangThai = !dichVu.TrangThai;
                _context.Update(dichVu);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái dịch vụ thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật trạng thái: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: DichVu/Delete/5 (Soft delete)
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var dichVu = await _context.DichVus.FirstOrDefaultAsync(m => m.MaDichVu == id);
            if (dichVu == null) return NotFound();
            return View(dichVu);
        }

        // POST: DichVu/Delete/5 (Gọi sp_XoaDichVu)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                // Gọi SP "sp_XoaDichVu" (soft delete)
                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_XoaDichVu @MaDichVu = {id}
                """);
                TempData["Success"] = "Xóa (ẩn) dịch vụ thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}