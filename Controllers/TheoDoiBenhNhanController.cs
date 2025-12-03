using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace App.Controllers
{
    [Authorize(Roles = "Role_Developer, Role_DevLead_DevOps, Admin")]
    public class TheoDoiBenhNhanController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TheoDoiBenhNhanController(QLBenhVienContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Hàm helper tải danh sách
        private void LoadViewData(string? selectedBenhNhan = null, string? selectedYTa = null)
        {
            ViewData["MaBenhNhan"] = new SelectList(
                _context.BenhNhans.Where(bn => bn.TrangThai == "Đang điều trị"),
                "MaBenhNhan", "HoTenBenhNhan", selectedBenhNhan);

            ViewData["MaYTa"] = new SelectList(
                _context.Yta.Where(yt => yt.TrangThai == true), // Dùng _context.Yta (viết thường)
                "MaYTa", "TenYTa", selectedYTa);
        }

        // GET: TheoDoiBenhNhan
        public async Task<IActionResult> Index(string maBenhNhan, string MaYTa, DateTime? tuNgay)
        {
            // SỬA LỖI 1: Dùng _context.TheoDoiBenhNhans (số nhiều)
            var query = _context.TheoDoiBenhNhans
                .Include(t => t.MaBenhNhanNavigation)
                // SỬA LỖI 2: Dùng MaYtaNavigation (viết thường 't')
                .Include(t => t.MaYtaNavigation)
                .OrderByDescending(t => t.NgayTheoDoi)
                .AsQueryable();

            if (!string.IsNullOrEmpty(maBenhNhan))
            {
                query = query.Where(t => t.MaBenhNhan == maBenhNhan);
                ViewData["MaBenhNhanFilter"] = maBenhNhan;
            }

            if (!string.IsNullOrEmpty(MaYTa))
            {
                // SỬA LỖI 2: Dùng MaYTa (viết thường 't')
                query = query.Where(t => t.MaYTa == MaYTa);
                ViewData["MaYTaFilter"] = MaYTa;
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(t => t.NgayTheoDoi.Value.Date == tuNgay.Value.Date);
                ViewData["TuNgayFilter"] = tuNgay.Value.ToString("yyyy-MM-dd");
            }

            LoadViewData(maBenhNhan, MaYTa);
            return View(await query.ToListAsync());
        }

        // GET: TheoDoiBenhNhan/Create
        public IActionResult Create(string id)
        {
            LoadViewData(id);
            var model = new TheoDoiBenhNhan
            {
                MaBenhNhan = id,
                NgayTheoDoi = DateTime.Now
            };
            return View(model);
        }

        // POST: TheoDoiBenhNhan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBenhNhan,MaYTa,NgayTheoDoi,NhietDo,HuyetAp,NhipTim,CanNang,ChieuCao,GhiChu,DanhGia,NhietDoHoiTho")] TheoDoiBenhNhan theoDoi)
        {
            ModelState.Remove("MaBenhNhanNavigation");
            // SỬA LỖI 2: Dùng MaYtaNavigation (viết thường 't')
            ModelState.Remove("MaYtaNavigation");
            ModelState.Remove("MaTheoDoi");
            if (string.IsNullOrEmpty(theoDoi.MaTheoDoi))
            {
                theoDoi.MaTheoDoi = "TD" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            }

            if (ModelState.IsValid)
            {
                try
                {
                
                    
                    _context.TheoDoiBenhNhans.Add(theoDoi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Thêm theo dõi thành công!";
                    return RedirectToAction(nameof(Index), new { maBenhNhan = theoDoi.MaBenhNhan });
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    string errorMessage = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage = ex.InnerException.Message;
                    }
                    ModelState.AddModelError("", "Lỗi CSDL: " + errorMessage);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            LoadViewData(theoDoi.MaBenhNhan, theoDoi.MaYTa);
            return View(theoDoi);
        }

        // GET: TheoDoiBenhNhan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            // SỬA LỖI 1: Dùng _context.TheoDoiBenhNhans (số nhiều)
            var theoDoi = await _context.TheoDoiBenhNhans.FindAsync(id);
            if (theoDoi == null) return NotFound();

            LoadViewData(theoDoi.MaBenhNhan, theoDoi.MaYTa); // SỬA LỖI 2
            return View(theoDoi);
        }

        // POST: TheoDoiBenhNhan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaTheoDoi,MaBenhNhan,MaYTa,NgayTheoDoi,NhietDo,HuyetAp,NhipTim,CanNang,ChieuCao,GhiChu,DanhGia,NhietDoHoiTho")] TheoDoiBenhNhan theoDoi)
        {
            if (id != theoDoi.MaTheoDoi) return NotFound();

            ModelState.Remove("MaBenhNhanNavigation");
            ModelState.Remove("MaYtaNavigation"); // SỬA LỖI 2

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(theoDoi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật theo dõi thành công!";
                    return RedirectToAction(nameof(Index), new { maBenhNhan = theoDoi.MaBenhNhan });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            LoadViewData(theoDoi.MaBenhNhan, theoDoi.MaYTa); // SỬA LỖI 2
            return View(theoDoi);
        }

        // GET: TheoDoiBenhNhan/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            // SỬA LỖI 1: Dùng _context.TheoDoiBenhNhans (số nhiều)
            var theoDoi = await _context.TheoDoiBenhNhans
                .Include(t => t.MaBenhNhanNavigation)
                .Include(t => t.MaYtaNavigation) // SỬA LỖI 2
                .FirstOrDefaultAsync(m => m.MaTheoDoi == id);
            if (theoDoi == null) return NotFound();
            return View(theoDoi);
        }

        // POST: TheoDoiBenhNhan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string maBenhNhan = null;
            try
            {
                // SỬA LỖI 1: Dùng _context.TheoDoiBenhNhans (số nhiều)
                var theoDoi = await _context.TheoDoiBenhNhans.FindAsync(id);
                if (theoDoi != null)
                {
                    maBenhNhan = theoDoi.MaBenhNhan;
                    _context.TheoDoiBenhNhans.Remove(theoDoi);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa theo dõi thành công.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index), new { maBenhNhan = maBenhNhan });
        }
    }
}