using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    // Chỉ DevLead và Admin mới được xem
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class LichSuHoatDongController : Controller
    {
        private readonly QLBenhVienContext _context;

        public LichSuHoatDongController(QLBenhVienContext context)
        {
            _context = context;
        }

        // GET: LichSuHoatDong (với bộ lọc)
        public async Task<IActionResult> Index(string? bangTacDong, string? hanhDong, DateTime? tuNgay)
        {
            // Dùng Index 'IX_LichSuHoatDong_ThoiGian_Desc' của bạn
            var query = _context.LichSuHoatDongs
                .OrderByDescending(l => l.ThoiGian)
                .AsQueryable();

            if (!string.IsNullOrEmpty(bangTacDong))
            {
                query = query.Where(l => l.BangTacDong == bangTacDong);
                ViewData["BangTacDongFilter"] = bangTacDong;
            }

            if (!string.IsNullOrEmpty(hanhDong))
            {
                query = query.Where(l => l.HanhDong == hanhDong);
                ViewData["HanhDongFilter"] = hanhDong;
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(l => l.ThoiGian.Value.Date == tuNgay.Value.Date);
                ViewData["TuNgayFilter"] = tuNgay.Value.ToString("yyyy-MM-dd");
            }

            // Lấy danh sách các Bảng đã bị tác động để làm bộ lọc
            ViewData["BangTacDongList"] = await _context.LichSuHoatDongs
                .Select(l => l.BangTacDong)
                .Distinct()
                .ToListAsync();

            // Chỉ lấy 500 dòng gần nhất để tránh quá tải
            var results = await query.Take(500).ToListAsync();

            return View(results);
        }

        // GET: LichSuHoatDong/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var lichSu = await _context.LichSuHoatDongs
                .FirstOrDefaultAsync(m => m.MaLichSu == id);
            if (lichSu == null) return NotFound();
            return View(lichSu);
        }
    }
}