using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.ViewModels;

namespace App.Controllers
{
    // 1. SỬA QUYỀN: Chỉ cho Admin và DevLead vào xem Dashboard (Nơi chạy báo cáo nhạy cảm)
    [Authorize(Roles = "Role_DevLead_DevOps, Admin,Role_ReadOnly_Analyst")]
    public class DashboardController : Controller
    {
        private readonly QLBenhVienContext _context;

        public DashboardController(QLBenhVienContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // === ĐOẠN CODE CÀI BẪY DEMO (QUAN TRỌNG) ===
            // Gọi Stored Procedure "sp_ThongKeTongQuan" để kích hoạt chế độ:
            // 1. SERIALIZABLE (Khóa toàn bộ bảng dữ liệu)
            // 2. WAITFOR DELAY (Giữ khóa trong 20s -> Tạo hiệu ứng "Load trang")
            try
            {
                // Dùng ExecuteSqlRawAsync để chạy SP. 
                // SP này sẽ khiến code C# dừng ở dòng này đúng 20 giây.
                // Trong 20 giây này, Trình duyệt sẽ quay vòng vòng (Loading...) -> Đây là dấu hiệu bạn cần.
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_ThongKeTongQuan");
            }
            catch (Exception)
            {
                // Bỏ qua lỗi nếu có (để demo không bị gián đoạn)
            }
            // =============================================


            // Sau khi chạy SP xong (hoặc song song), load dữ liệu hiển thị lên View
            // 1. Lấy thống kê
            var thongKe = await _context.VwThongKeHoatDongs.FirstOrDefaultAsync();

            // 2. Lấy Lịch hẹn hôm nay
            var lichHen = await _context.VwLichHenHomNays
                                .OrderBy(l => l.NgayHen)
                                .ToListAsync();

            // 3. Lấy Thuốc sắp hết hạn
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