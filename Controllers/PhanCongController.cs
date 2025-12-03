using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class PhanCongController : Controller
    {
        private readonly QLBenhVienContext _context;

        public PhanCongController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Hàm helper để tải danh sách (Bác sĩ, Y tá, Phòng)
        private void LoadViewData()
        {
            ViewData["MaBacSi"] = new SelectList(
                _context.BacSis.Where(b => b.TrangThai == true),
                "MaBacSi", "TenBacSi");

            ViewData["MaYTa"] = new SelectList(
                _context.Yta.Where(y => y.TrangThai == true),
                "MaYTa", "TenYTa");

            ViewData["MaPhong"] = new SelectList(
                _context.PhongBenhs.Where(p => p.TrangThai == true),
                "MaPhong", "TenPhong");
        }

        // GET: PhanCong
        public async Task<IActionResult> Index()
        {
            var phanCongs = _context.PhanCongs
                .Include(p => p.MaBacSiNavigation)
                .Include(p => p.MaPhongNavigation)
                .Include(p => p.MaYtaNavigation)
                .OrderByDescending(p => p.NgayPhanCong);

            return View(await phanCongs.ToListAsync());
        }

        // GET: PhanCong/Create
        public IActionResult Create()
        {
            LoadViewData();
            var model = new PhanCong
            {
                TrangThai = "Đang làm" // Mặc định
            };
            return View(model);
        }

        // POST: PhanCong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhanCong,MaBacSi,MaYta,MaPhong,NgayPhanCong,GhiChu,TrangThai")] PhanCong phanCong)
        {
            ModelState.Remove("MaBacSiNavigation");
            ModelState.Remove("MaYtaNavigation");
            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(phanCong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo phân công mới thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            LoadViewData();
            return View(phanCong);
        }

        // GET: PhanCong/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var phanCong = await _context.PhanCongs.FindAsync(id);
            if (phanCong == null) return NotFound();

            LoadViewData();
            return View(phanCong);
        }

        // POST: PhanCong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaPhanCong,MaBacSi,MaYta,MaPhong,NgayPhanCong,GhiChu,TrangThai")] PhanCong phanCong)
        {
            if (id != phanCong.MaPhanCong) return NotFound();

            ModelState.Remove("MaBacSiNavigation");
            ModelState.Remove("MaYtaNavigation");
            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phanCong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phân công thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            LoadViewData();
            return View(phanCong);
        }

        // GET: PhanCong/Cancel/5 (Hủy phân công)
        public async Task<IActionResult> Cancel(string id)
        {
            if (id == null) return NotFound();
            var phanCong = await _context.PhanCongs
                .Include(p => p.MaBacSiNavigation)
                .Include(p => p.MaYtaNavigation)
                .Include(p => p.MaPhongNavigation)
                .FirstOrDefaultAsync(m => m.MaPhanCong == id);
            if (phanCong == null) return NotFound();
            return View(phanCong);
        }

        // POST: PhanCong/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelConfirmed(string id)
        {
            try
            {
                var phanCong = await _context.PhanCongs.FindAsync(id);
                if (phanCong != null)
                {
                    phanCong.TrangThai = "Hủy"; // Cập nhật trạng thái
                    _context.Update(phanCong);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Hủy phân công thành công.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi hủy: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}