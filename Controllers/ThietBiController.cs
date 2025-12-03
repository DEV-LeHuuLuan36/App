using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class ThietBiController : Controller
    {
        private readonly QLBenhVienContext _context;

        public ThietBiController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Hàm helper để tải danh sách Phòng bệnh
        private void LoadPhongList()
        {
            // Tải tất cả phòng (thiết bị có thể ở phòng kho, không chỉ phòng bệnh)
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong");
        }

        // GET: ThietBi (Gọi sp_LayDanhSachThietBi)
        public async Task<IActionResult> Index()
        {
            // SP 'sp_LayDanhSachThietBi' trả về TenPhong, TenKhoa, không khớp Model 'ThietBi'
            // Chúng ta sẽ dùng EF Core Include để thay thế, vẫn đảm bảo là T-SQL
            var thietBis = _context.ThietBis
                .Include(t => t.MaPhongNavigation)
                .ThenInclude(p => p.MaKhoaNavigation)
                .OrderBy(t => t.TenThietBi);

            return View(await thietBis.ToListAsync());
        }

        // GET: ThietBi/Create
        public IActionResult Create()
        {
            LoadPhongList();
            return View();
        }

        // POST: ThietBi/Create (Gọi sp_ThemThietBi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThietBi,TenThietBi,MaPhong,NgayMua,HanBaoHanh,TinhTrang,GhiChu,DonGia")] ThietBi thietBi)
        {
            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi SP "sp_ThemThietBi"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_ThemThietBi
                            @MaThietBi = {thietBi.MaThietBi},
                            @TenThietBi = {thietBi.TenThietBi},
                            @MaPhong = {thietBi.MaPhong},
                            @NgayMua = {thietBi.NgayMua},
                            @HanBaoHanh = {thietBi.HanBaoHanh},
                            @TinhTrang = {thietBi.TinhTrang},
                            @GhiChu = {thietBi.GhiChu}
                    """);

                    // Cập nhật trường DonGia (SP của bạn không có)
                    _context.Update(thietBi);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm thiết bị thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            LoadPhongList();
            return View(thietBi);
        }

        // GET: ThietBi/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var thietBi = await _context.ThietBis.FindAsync(id);
            if (thietBi == null) return NotFound();

            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong", thietBi.MaPhong);
            return View(thietBi);
        }

        // POST: ThietBi/Edit/5 (Gọi sp_CapNhatThietBi)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaThietBi,TenThietBi,MaPhong,NgayMua,HanBaoHanh,TinhTrang,GhiChu,DonGia,PhongBan,NgayKiemKe,NguoiQuanLy")] ThietBi thietBi)
        {
            if (id != thietBi.MaThietBi) return NotFound();

            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Gọi SP "sp_CapNhatThietBi"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_CapNhatThietBi
                            @MaThietBi = {thietBi.MaThietBi},
                            @TenThietBi = {thietBi.TenThietBi},
                            @MaPhong = {thietBi.MaPhong},
                            @NgayMua = {thietBi.NgayMua},
                            @HanBaoHanh = {thietBi.HanBaoHanh},
                            @TinhTrang = {thietBi.TinhTrang},
                            @GhiChu = {thietBi.GhiChu}
                    """);

                    // 2. Cập nhật các trường còn lại bằng EF Core
                    _context.Update(thietBi);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật thiết bị thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong", thietBi.MaPhong);
            return View(thietBi);
        }

        // GET: ThietBi/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var thietBi = await _context.ThietBis
                .Include(t => t.MaPhongNavigation)
                .FirstOrDefaultAsync(m => m.MaThietBi == id);
            if (thietBi == null) return NotFound();
            return View(thietBi);
        }

        // POST: ThietBi/Delete/5 (Gọi sp_XoaThietBi)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                // Gọi SP "sp_XoaThietBi" (hard delete)
                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_XoaThietBi @MaThietBi = {id}
                """);
                TempData["Success"] = "Xóa thiết bị thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}