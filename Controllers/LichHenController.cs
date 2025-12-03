using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    // Yêu cầu Role_Developer trở lên
    [Authorize(Roles = "Role_Developer, Role_DevLead_DevOps, Admin")]
    public class LichHenController : Controller
    {
        private readonly QLBenhVienContext _context;

        public LichHenController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Hàm helper để tải các SelectList
        private void LoadViewData()
        {
            ViewData["MaBenhNhan"] = new SelectList(_context.BenhNhans, "MaBenhNhan", "HoTenBenhNhan");
            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa");
            ViewData["MaBacSi"] = new SelectList(_context.BacSis, "MaBacSi", "TenBacSi");
        }

        // GET: LichHen
        public async Task<IActionResult> Index()
        {
            var lichHens = _context.LichHens
                .Include(l => l.MaBacSiNavigation)
                .Include(l => l.MaBenhNhanNavigation)
                .Include(l => l.MaKhoaNavigation)
                .OrderByDescending(l => l.NgayHen); // Sắp xếp
            return View(await lichHens.ToListAsync());
        }

        // GET: LichHen/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var lichHen = await _context.LichHens
                .Include(l => l.MaBacSiNavigation)
                .Include(l => l.MaBenhNhanNavigation)
                .Include(l => l.MaKhoaNavigation)
                .FirstOrDefaultAsync(m => m.MaLichHen == id);
            if (lichHen == null) return NotFound();
            return View(lichHen);
        }

        // GET: LichHen/Create
        public IActionResult Create()
        {
            LoadViewData();
            return View();
        }

        // POST: LichHen/Create (Gọi sp_DatLichHen)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLichHen,MaBenhNhan,MaKhoa,NgayHen,LyDoKham,MaBacSi")] LichHen lichHen)
        {
            ModelState.Remove("MaBenhNhanNavigation");
            ModelState.Remove("MaKhoaNavigation");
            ModelState.Remove("MaBacSiNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi SP "sp_DatLichHen"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_DatLichHen
                            @MaLichHen = {lichHen.MaLichHen},
                            @MaBenhNhan = {lichHen.MaBenhNhan},
                            @MaKhoa = {lichHen.MaKhoa},
                            @NgayHen = {lichHen.NgayHen},
                            @LyDoKham = {lichHen.LyDoKham},
                            @MaBacSi = {lichHen.MaBacSi}
                    """);
                    TempData["Success"] = "Đặt lịch hẹn thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi đặt lịch: " + ex.Message);
                }
            }
            LoadViewData();
            return View(lichHen);
        }

        // GET: LichHen/XacNhan/5 (Màn hình Xác nhận)
        public async Task<IActionResult> XacNhan(string id)
        {
            if (id == null) return NotFound();
            var lichHen = await _context.LichHens
                .Include(l => l.MaBenhNhanNavigation)
                .FirstOrDefaultAsync(m => m.MaLichHen == id);
            if (lichHen == null) return NotFound();

            // Chỉ load Bác sĩ thuộc Khoa của lịch hẹn
            ViewData["MaBacSi"] = new SelectList(_context.BacSis.Where(b => b.MaKhoa == lichHen.MaKhoa), "MaBacSi", "TenBacSi");
            return View(lichHen);
        }

        // POST: LichHen/XacNhan/5 (Gọi sp_XacNhanLichHen)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(string MaLichHen, string MaBacSi)
        {
            if (MaLichHen == null || MaBacSi == null)
            {
                TempData["Error"] = "Phải chọn bác sĩ để xác nhận.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Gọi SP "sp_XacNhanLichHen"
                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_XacNhanLichHen
                        @MaLichHen = {MaLichHen},
                        @MaBacSi = {MaBacSi}
                """);
                TempData["Success"] = "Xác nhận lịch hẹn thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xác nhận: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: LichHen/Huy/5 (Màn hình Hủy)
        public async Task<IActionResult> Huy(string id)
        {
            if (id == null) return NotFound();
            var lichHen = await _context.LichHens
                .Include(l => l.MaBenhNhanNavigation)
                .FirstOrDefaultAsync(m => m.MaLichHen == id);
            if (lichHen == null) return NotFound();
            return View(lichHen);
        }

        // POST: LichHen/Huy/5 (Cập nhật trạng thái)
        [HttpPost, ActionName("Huy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyConfirmed(string id)
        {
            var lichHen = await _context.LichHens.FindAsync(id);
            if (lichHen != null)
            {
                lichHen.TrangThai = "Hủy"; // Trạng thái "Hủy"
                _context.Update(lichHen);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã hủy lịch hẹn.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Hàm này Scaffolding tạo ra nhưng ta không dùng
        // (Chúng ta không Sửa lịch hẹn, chỉ Hủy hoặc Xác nhận)
        // Bạn có thể xóa các hàm Edit và Delete đi nếu muốn.
    }
}