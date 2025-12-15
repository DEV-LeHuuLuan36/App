using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using App.Models;

namespace App.Controllers
{
    public class TestConcurrencyController : Controller
    {
        private readonly IConfiguration _configuration;

        public TestConcurrencyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return Content(@"
                <h1>TRANG DEMO TRANSACTION ISOLATION</h1>
                <p>Mở 2 Tab trình duyệt để test.</p>
                <hr/>
                <h3>Kịch bản 1: READ COMMITTED (Mặc định) - Demo Sửa Bác Sĩ</h3>
                <p>Tab 1 bấm link này (Sẽ giữ khóa 15s): <a href='/TestConcurrency/DemoReadCommitted' target='_blank'>1. Bắt đầu Transaction (Treo 15s)</a></p>
                <p>Trong lúc Tab 1 quay, qua Tab 2 vào trang Quản lý Bác sĩ sửa thông tin -> Sẽ thấy bị treo.</p>
                <hr/>
                <h3>Kịch bản 2: SERIALIZABLE - Demo Báo Cáo</h3>
                <p>Tab 1 bấm link này (Khóa chặt bảng 15s): <a href='/TestConcurrency/DemoSerializable' target='_blank'>2. Chạy Báo Cáo (Treo 15s)</a></p>
                <p>Trong lúc Tab 1 quay, qua Tab 2 thử Tạo mới Hóa đơn -> Sẽ thấy bị treo.</p>
            ", "text/html");
        }

        // DEMO 1: READ COMMITTED (Update dữ liệu và giữ khóa dòng)
        public async Task<IActionResult> DemoReadCommitted()
        {
            string connString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                // Mặc định là ReadCommitted
                using (var trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        // Giả lập update 1 dòng (Thay 'NV00000001' bằng mã thật trong DB của bạn)
                        var cmd = new SqlCommand("UPDATE BacSi SET TenBacSi = N'Dr. Demo Lock' WHERE MaBacSi = 'NV00000001'", conn, trans);
                        await cmd.ExecuteNonQueryAsync();

                        // QUAN TRỌNG: Giữ khóa 15 giây để thầy cô kịp nhìn thấy
                        await Task.Delay(15000);

                        trans.Commit();
                        return Content("Xong Transaction ReadCommitted! Đã nhả khóa.");
                    }
                    catch { trans.Rollback(); return Content("Lỗi"); }
                }
            }
        }

        // DEMO 2: SERIALIZABLE (Khóa phạm vi, chặn Insert)
        public async Task<IActionResult> DemoSerializable()
        {
            string connString = _configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                // Thiết lập mức SERIALIZABLE
                using (var trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        // Đọc dữ liệu (Sẽ khóa dải dữ liệu này lại)
                        var cmd = new SqlCommand("SELECT COUNT(*) FROM HoaDon WHERE NgayLap = CAST(GETDATE() AS DATE)", conn, trans);
                        await cmd.ExecuteScalarAsync();

                        // QUAN TRỌNG: Giữ khóa 15 giây
                        await Task.Delay(15000);

                        trans.Commit();
                        return Content("Xong Transaction Serializable! Đã nhả khóa.");
                    }
                    catch { trans.Rollback(); return Content("Lỗi"); }
                }
            }
        }
    }
}