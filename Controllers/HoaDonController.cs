using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.ViewModels; // <-- Thêm ViewModel
using Microsoft.AspNetCore.Identity; // <-- Thêm Identity

namespace App.Controllers
{
    [Authorize(Roles = "Role_Developer, Role_DevLead_DevOps, Admin")]
    public class HoaDonController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HoaDonController(QLBenhVienContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HoaDon
        public async Task<IActionResult> Index()
        {
            var hoaDons = _context.HoaDons
                .Include(h => h.MaBenhNhanNavigation)
                .OrderByDescending(h => h.NgayLap);
            return View(await hoaDons.ToListAsync());
        }

        // GET: HoaDon/Create
        // Trang này nên được gọi từ Bệnh nhân, nhưng ta làm trang độc lập
        public IActionResult Create()
        {
            ViewData["MaBenhNhan"] = new SelectList(_context.BenhNhans, "MaBenhNhan", "HoTenBenhNhan");
            return View();
        }

        // POST: HoaDon/Create (Gọi sp_TaoHoaDon)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaHoaDon,MaBenhNhan")] HoaDon hoaDon)
        {
            ModelState.Remove("MaBenhNhanNavigation");

            // Lấy ID của nhân viên đang đăng nhập (ví dụ: nhân viên thu ngân)
            var userId = _userManager.GetUserId(User);
            // TODO: Bạn cần một logic để map AspNetUserId sang MaNhanVien
            string maNhanVienThuNgan = "NV001"; // Tạm thời hard-code

            if (ModelState.IsValid)
            {
                try
                {
                    // SP "sp_TaoHoaDon" của bạn không có MaNhanVien,
                    // nhưng Bảng HoaDon lại có. Tôi sẽ thêm MaNhanVien vào.
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        INSERT INTO HoaDon (MaHoaDon, MaBenhNhan, MaNhanVien, NgayLap, TrangThai)
                        VALUES ({hoaDon.MaHoaDon}, {hoaDon.MaBenhNhan}, {maNhanVienThuNgan}, GETDATE(), N'Chưa thanh toán')
                    """);

                    TempData["Success"] = "Tạo hóa đơn mới thành công!";
                    // Chuyển ngay đến trang Chi tiết để thêm Dịch vụ/Thuốc
                    return RedirectToAction(nameof(Details), new { id = hoaDon.MaHoaDon });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi tạo hóa đơn: " + ex.Message);
                }
            }
            ViewData["MaBenhNhan"] = new SelectList(_context.BenhNhans, "MaBenhNhan", "HoTenBenhNhan", hoaDon.MaBenhNhan);
            return View(hoaDon);
        }

        // GET: HoaDon/Details/HD001
        // Trang chính để quản lý (Thêm dịch vụ, thêm thuốc, thanh toán)
        public async Task<IActionResult> Details(string id) // id = MaHoaDon
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaBenhNhanNavigation)
                .Include(h => h.MaNhanVienNavigation)
                .FirstOrDefaultAsync(m => m.MaHoaDon == id);

            if (hoaDon == null) return NotFound();

            var chiTiet = await _context.ChiTietHoaDons
                .Include(ct => ct.MaDichVuNavigation)
                .Include(ct => ct.MaDonThuocNavigation)
                .Where(ct => ct.MaHoaDon == id)
                .ToListAsync();

            // Lấy danh sách Dịch vụ (để thêm)
            ViewData["DichVuList"] = new SelectList(_context.DichVus.Where(dv => dv.TrangThai == true), "MaDichVu", "TenDichVu");

            // Lấy danh sách Đơn thuốc CHƯA thanh toán của bệnh nhân này
            var donThuocChuaThanhToan = await _context.DonThuocs
                .Where(dt => dt.MaBenhNhan == hoaDon.MaBenhNhan &&
                             !_context.ChiTietHoaDons.Any(ct => ct.MaDonThuoc == dt.MaDonThuoc))
                .Select(dt => new { dt.MaDonThuoc, NgayKe = dt.NgayKeDon })
                .Distinct()
                .ToListAsync();

            ViewData["DonThuocList"] = new SelectList(donThuocChuaThanhToan, "MaDonThuoc", "MaDonThuoc"); // Tạm thời

            var viewModel = new HoaDonViewModel
            {
                HoaDon = hoaDon,
                ChiTietHoaDon = chiTiet
            };

            return View(viewModel);
        }

        // POST: HoaDon/ThemDichVu (Gọi sp_ThemDichVuHoaDon)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemDichVu(string MaHoaDon, string MaDichVu, int SoLuong)
        {
            if (MaHoaDon == null || MaDichVu == null || SoLuong <= 0)
            {
                TempData["Error"] = "Vui lòng chọn Dịch vụ và Số lượng hợp lệ.";
                return RedirectToAction(nameof(Details), new { id = MaHoaDon });
            }

            try
            {
                string maCTHD = "CT" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Tạo mã CT ngẫu nhiên

                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_ThemDichVuHoaDon
                        @MaCTHD = {maCTHD},
                        @MaHoaDon = {MaHoaDon},
                        @MaDichVu = {MaDichVu},
                        @SoLuong = {SoLuong}
                """);
                TempData["Success"] = "Thêm dịch vụ thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi thêm dịch vụ: " + ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = MaHoaDon });
        }

        // POST: HoaDon/ThemThuoc (Gọi sp_ThemThuocHoaDon - SP mới)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemThuoc(string MaHoaDon, string MaDonThuoc)
        {
            if (MaHoaDon == null || MaDonThuoc == null)
            {
                TempData["Error"] = "Vui lòng chọn Đơn thuốc.";
                return RedirectToAction(nameof(Details), new { id = MaHoaDon });
            }

            try
            {
                string maCTHD = "CT" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(); // Tạo mã CT ngẫu nhiên

                // Gọi SP mới ta vừa tạo
                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_ThemThuocHoaDon
                        @MaCTHD = {maCTHD},
                        @MaHoaDon = {MaHoaDon},
                        @MaDonThuoc = {MaDonThuoc}
                """);
                TempData["Success"] = "Thêm đơn thuốc vào hóa đơn thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi thêm thuốc: " + ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = MaHoaDon });
        }

        // GET: HoaDon/ThanhToan/HD001
        public async Task<IActionResult> ThanhToan(string id)
        {
            if (id == null) return NotFound();
            var hoaDon = await _context.HoaDons
                .Include(h => h.MaBenhNhanNavigation)
                .FirstOrDefaultAsync(h => h.MaHoaDon == id);

            if (hoaDon == null) return NotFound();
            if (hoaDon.TrangThai == "Đã thanh toán")
            {
                TempData["Error"] = "Hóa đơn này đã được thanh toán rồi.";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(hoaDon);
        }

        // POST: HoaDon/ThanhToan (Gọi sp_ThanhToanHoaDon)
        [HttpPost, ActionName("ThanhToan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToanConfirmed(string MaHoaDon, string PhuongThucTT)
        {
            if (MaHoaDon == null || PhuongThucTT == null)
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Gọi SP "sp_ThanhToanHoaDon"
                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_ThanhToanHoaDon
                        @MaHoaDon = {MaHoaDon},
                        @PhuongThucTT = {PhuongThucTT}
                """);
                TempData["Success"] = "Thanh toán hóa đơn thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi thanh toán: " + ex.Message;
            }
            return RedirectToAction(nameof(Details), new { id = MaHoaDon });
        }
    }
}