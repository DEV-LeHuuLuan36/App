using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    // CHỈ DEVLEAD VÀ ADMIN MỚI ĐƯỢC VÀO
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class MaintenanceController : Controller
    {
        private readonly QLBenhVienContext _context;

        public MaintenanceController(QLBenhVienContext context)
        {
            _context = context;
        }

        // Trang Index của bảo trì
        public IActionResult Index()
        {
            return View();
        }

        // POST: Chạy Cursor (Kỹ thuật T-SQL)
        [HttpPost]
        [ValidateAntiForgeryToken]
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