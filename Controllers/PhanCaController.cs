using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class PhanCaController : Controller
    {
        private readonly QLBenhVienContext _context;

        public PhanCaController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Hàm helper để tải danh sách (Ca và Nhân viên)
        private void LoadViewData(string? selectedNhanVien = null, string? selectedCa = null)
        {
            // Lấy tất cả nhân viên đang làm việc
            ViewData["MaNhanVien"] = new SelectList(
                _context.NhanViens.Where(n => n.TrangThai == true),
                "MaNhanVien", "HoTen", selectedNhanVien);

            ViewData["MaCa"] = new SelectList(
                _context.CaTrucs,
                "MaCa", "TenCa", selectedCa);
        }

        // GET: PhanCa
        public async Task<IActionResult> Index(DateTime? ngayLamViec)
        {
            // Mặc định hiển thị lịch của hôm nay
            var ngayFilter = ngayLamViec ?? DateTime.Today;
            ViewData["NgayLamViecFilter"] = ngayFilter.ToString("yyyy-MM-dd");

            var phanCas = _context.PhanCas
                .Include(p => p.MaCaNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .Where(p => p.NgayLamViec == ngayFilter) // Lọc theo ngày
                .OrderBy(p => p.MaCaNavigation.GioBatDau);

            return View(await phanCas.ToListAsync());
        }

        // GET: PhanCa/Create
        public IActionResult Create(DateTime? ngay)
        {
            LoadViewData();
            var model = new PhanCa
            {
                // Gán sẵn ngày (nếu được truyền từ Index)
                NgayLamViec = ngay ?? DateTime.Today,
                TrangThai = "Đã phân" // Mặc định
            };
            return View(model);
        }

        // POST: PhanCa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhanCa,MaNhanVien,MaCa,NgayLamViec,GhiChu,TrangThai")] PhanCa phanCa)
        {
            ModelState.Remove("MaNhanVienNavigation");
            ModelState.Remove("MaCaNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(phanCa);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Phân ca mới thành công!";
                    return RedirectToAction(nameof(Index), new { ngayLamViec = phanCa.NgayLamViec });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            LoadViewData(phanCa.MaNhanVien, phanCa.MaCa);
            return View(phanCa);
        }

        // GET: PhanCa/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var phanCa = await _context.PhanCas.FindAsync(id);
            if (phanCa == null) return NotFound();

            LoadViewData(phanCa.MaNhanVien, phanCa.MaCa);
            return View(phanCa);
        }

        // POST: PhanCa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaPhanCa,MaNhanVien,MaCa,NgayLamViec,GhiChu,TrangThai")] PhanCa phanCa)
        {
            if (id != phanCa.MaPhanCa) return NotFound();

            ModelState.Remove("MaNhanVienNavigation");
            ModelState.Remove("MaCaNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanCa);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phân ca thành công!";
                    return RedirectToAction(nameof(Index), new { ngayLamViec = phanCa.NgayLamViec });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            LoadViewData(phanCa.MaNhanVien, phanCa.MaCa);
            return View(phanCa);
        }

        // GET: PhanCa/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var phanCa = await _context.PhanCas
                .Include(p => p.MaCaNavigation)
                .Include(p => p.MaNhanVienNavigation)
                .FirstOrDefaultAsync(m => m.MaPhanCa == id);
            if (phanCa == null) return NotFound();
            return View(phanCa);
        }

        // POST: PhanCa/Delete/5 (Hard Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            DateTime? ngayLamViec = DateTime.Today;
            try
            {
                var phanCa = await _context.PhanCas.FindAsync(id);
                if (phanCa != null)
                {
                    ngayLamViec = phanCa.NgayLamViec;
                    _context.PhanCas.Remove(phanCa);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa phân ca thành công.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index), new { ngayLamViec = ngayLamViec });
        }
    }
}