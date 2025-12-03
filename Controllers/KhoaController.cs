using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize]
    public class KhoaController : Controller
    {
        private readonly QLBenhVienContext _context;

        public KhoaController(QLBenhVienContext context) { _context = context; }

        [Authorize(Roles = "Role_ReadOnly_Analyst, Role_Developer, Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả, kể cả khoa đã ẩn (TrangThai = 0)
            return View(await _context.Khoas.OrderBy(k => k.TenKhoa).ToListAsync());
        }

        [Authorize(Roles = "Role_ReadOnly_Analyst, Role_Developer, Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var khoa = await _context.Khoas.FirstOrDefaultAsync(m => m.MaKhoa == id);
            if (khoa == null) return NotFound();
            return View(khoa);
        }

        [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> Create([Bind("MaKhoa,TenKhoa,MoTa,TruongKhoa,NgayThanhLap,TrangThai")] Khoa khoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khoa);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Tạo khoa mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(khoa);
        }

        [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var khoa = await _context.Khoas.FindAsync(id);
            if (khoa == null) return NotFound();
            return View(khoa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhoa,TenKhoa,MoTa,TruongKhoa,NgayThanhLap,TrangThai")] Khoa khoa)
        {
            if (id != khoa.MaKhoa) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khoa);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật khoa thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Khoas.Any(e => e.MaKhoa == khoa.MaKhoa))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(khoa);
        }

        // --- THÊM HÀM MỚI NÀY ---
        // POST: Khoa/ToggleStatus/KHOA01
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            var khoa = await _context.Khoas.FindAsync(id);
            if (khoa == null) return NotFound();

            try
            {
                // Lật ngược trạng thái
                khoa.TrangThai = !khoa.TrangThai;
                _context.Update(khoa);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái Khoa thành công.";
            }
            catch (Exception ex)
            {
                // Bắt lỗi nếu Khoa đang có Phòng
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // --- BỎ HÀM DELETE VÀ DELETECONFIRMED ---
    }
}