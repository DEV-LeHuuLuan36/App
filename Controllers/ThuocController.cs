using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class ThuocController : Controller
    {
        private readonly QLBenhVienContext _context;

        public ThuocController(QLBenhVienContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string timKiem)
        {
            // Lấy tất cả thuốc, kể cả thuốc đã ẩn (TrangThai = 0)
            var query = _context.Thuocs.AsQueryable();

            if (!string.IsNullOrEmpty(timKiem))
            {
                query = query.Where(t => t.TenThuoc.Contains(timKiem));
                ViewData["timKiemHienTai"] = timKiem;
            }

            return View(await query.OrderBy(t => t.TenThuoc).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThuoc,TenThuoc,DonViTinh,DonGia,SoLuongTon,HanSuDung,ChongChiDinh,NhaSanXuat,LoaiThuoc,TrangThai")] Thuoc thuoc)
        {
            // Gán TrangThai = 1 (Active) khi tạo mới
            thuoc.TrangThai = true;

            if (ModelState.IsValid)
            {
                try
                {
                    // SP sp_ThemThuoc của bạn thiếu nhiều trường, dùng EF Core
                    _context.Add(thuoc);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm thuốc thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            return View(thuoc);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound();
            return View(thuoc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaThuoc,TenThuoc,DonViTinh,DonGia,SoLuongTon,HanSuDung,ChongChiDinh,NhaSanXuat,LoaiThuoc,TrangThai")] Thuoc thuoc)
        {
            if (id != thuoc.MaThuoc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // SP sp_CapNhatThuoc của bạn thiếu nhiều trường
                    _context.Update(thuoc);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật thuốc thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            return View(thuoc);
        }

        // --- THÊM HÀM MỚI NÀY ---
        // POST: Thuoc/ToggleStatus/T001
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            var thuoc = await _context.Thuocs.FindAsync(id);
            if (thuoc == null) return NotFound();

            try
            {
                thuoc.TrangThai = !thuoc.TrangThai;
                _context.Update(thuoc);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái Thuốc thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // --- BỎ HÀM DELETE VÀ DELETECONFIRMED ---
    }
}