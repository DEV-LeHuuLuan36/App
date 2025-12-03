using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.ViewModels; // <-- Thêm ViewModel
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Controllers
{
    // Analyst và các cấp cao hơn đều được vào
    [Authorize(Roles = "Role_ReadOnly_Analyst, Role_Developer, Role_DevLead_DevOps, Admin")]
    public class BaoCaoController : Controller
    {
        private readonly QLBenhVienContext _context;

        public BaoCaoController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Trang Index: Cổng thông tin đến các báo cáo
        public IActionResult Index()
        {
            return View();
        }

        // --- KỸ THUẬT 1: GỌI SP CÓ THAM SỐ (sp_BaoCaoDoanhThu) ---
        [HttpGet]
        public IActionResult BaoCaoDoanhThu()
        {
            // Chỉ trả về View trống
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaoCaoDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            // Gọi T-SQL (SP)
            var results = await _context.Set<BaoCaoDoanhThuResult>()
                .FromSqlInterpolated($"""
                    EXEC sp_BaoCaoDoanhThu
                        @TuNgay = {tuNgay},
                        @DenNgay = {denNgay}
                """).ToListAsync();

            ViewData["TuNgay"] = tuNgay.ToString("dd/MM/yyyy");
            ViewData["DenNgay"] = denNgay.ToString("dd/MM/yyyy");
            return View(results);
        }

        // --- KỸ THUẬT 2 & 3: GỌI SP (GROUP BY, AVG) & HÀM (fn_TinhTuoi) ---
        [HttpGet]
        public async Task<IActionResult> ThongKeBenhTheoKhoa(string maKhoa)
        {
            ViewData["MaKhoaList"] = new SelectList(_context.Khoas.Where(k => k.TrangThai == true), "MaKhoa", "TenKhoa", maKhoa);
            ViewData["MaKhoaChon"] = maKhoa;

            // Gọi T-SQL (SP)
            var results = await _context.Set<ThongKeBenhResult>()
                .FromSqlInterpolated($"""
                    EXEC sp_ThongKeBenhTheoKhoa @MaKhoa = {maKhoa}
                """).ToListAsync();

            return View(results);
        }

        // --- KỸ THUẬT 4: GỌI VIEW (vw_ThuocSapHetHan) ---
        [HttpGet]
        public async Task<IActionResult> ThuocSapHetHan()
        {
            // Gọi T-SQL (View)
            var results = await _context.VwThuocSapHetHans.ToListAsync();
            return View(results);
        }

        // --- KỸ THUẬT 5: GỌI SP CÓ CURSOR ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Role_DevLead_DevOps, Admin")] // Chỉ Admin mới được chạy
        public async Task<IActionResult> RunCursorCapNhatGiuong()
        {
            try
            {
                // Gọi T-SQL (SP với Cursor)
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_CapNhatGiuongTrong_Cursor");
                TempData["Success"] = "Chạy Cursor cập nhật giường trống thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi chạy Cursor: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}