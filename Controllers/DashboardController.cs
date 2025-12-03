using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.ViewModels; // <-- Đổi namespace

namespace App.Controllers
{
    [Authorize] // Bắt buộc đăng nhập
    public class DashboardController : Controller
    {
        private readonly QLBenhVienContext _context;

        public DashboardController(QLBenhVienContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Kỹ thuật T-SQL (View): Lấy 4 thẻ KPI
            // Dùng .FirstOrDefault() vì View này chỉ trả về 1 dòng
            var thongKe = await _context.VwThongKeHoatDongs.FirstOrDefaultAsync();

            // 2. Kỹ thuật T-SQL (View): Lấy Lịch hẹn hôm nay
            var lichHen = await _context.VwLichHenHomNays
                                .OrderBy(l => l.NgayHen)
                                .ToListAsync();

            // 3. Kỹ thuật T-SQL (View): Lấy Thuốc sắp hết hạn
            var thuoc = await _context.VwThuocSapHetHans
                                .OrderBy(t => t.SoNgayConLai)
                                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                ThongKeHoatDong = thongKe ?? new VwThongKeHoatDong(),
                LichHenHomNay = lichHen,
                ThuocSapHetHan = thuoc
            };

            return View(viewModel);
        }
    }
}