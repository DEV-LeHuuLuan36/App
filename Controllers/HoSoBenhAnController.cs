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
    public class HoSoBenhAnController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HoSoBenhAnController(QLBenhVienContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HoSoBenhAn/Index/BN001
        // (id = MaBenhNhan)
        public async Task<IActionResult> Index(string id)
        {
            if (id == null) return NotFound();

            var benhNhan = await _context.BenhNhans
                .Include(b => b.MaPhongNavigation)
                .FirstOrDefaultAsync(b => b.MaBenhNhan == id);

            if (benhNhan == null) return NotFound();

            // Lấy danh sách bệnh án và điều trị của bệnh nhân này
            var danhSachHoSo = await _context.HoSoBenhAns
                .Where(h => h.MaBenhNhan == id)
                .OrderByDescending(h => h.NgayLap)
                .ToListAsync();

            var danhSachDieuTri = await _context.DieuTris
                .Include(d => d.MaBacSiNavigation) // Lấy tên Bác sĩ
                .Where(d => d.MaBenhNhan == id)
                .OrderByDescending(d => d.NgayDieuTri)
                .ToListAsync();

            // Tạo ViewModel để gửi 3 đối tượng này qua View
            var viewModel = new BenhAnViewModel
            {
                BenhNhan = benhNhan,
                DanhSachHoSoBenhAn = danhSachHoSo,
                DanhSachDieuTri = danhSachDieuTri
            };

            return View(viewModel);
        }

        // GET: HoSoBenhAn/TaoHoSo/BN001
        public IActionResult TaoHoSo(string maBenhNhan)
        {
            if (maBenhNhan == null) return NotFound();

            // Lấy MaBacSi từ User đang đăng nhập (giả định User đã được map với BacSi)
            // Tạm thời chúng ta sẽ load 1 list Bác sĩ
            ViewData["MaBacSi"] = new SelectList(_context.BacSis, "MaBacSi", "TenBacSi");
            ViewData["MaBenhNhan"] = maBenhNhan;

            return View();
        }

        // POST: HoSoBenhAn/TaoHoSo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoHoSo([Bind("MaHS,MaBenhNhan,ChuanDoan,TinhTrang,TrieuChung,TienSuBenh,MaBacSi")] HoSoBenhAn hoSoBenhAn)
        {
            ModelState.Remove("MaBenhNhanNavigation");
            ModelState.Remove("MaBacSiNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi SP "sp_TaoHoSoBenhAn"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_TaoHoSoBenhAn
                            @MaHS = {hoSoBenhAn.MaHs},
                            @MaBenhNhan = {hoSoBenhAn.MaBenhNhan},
                            @ChuanDoan = {hoSoBenhAn.ChuanDoan},
                            @TinhTrang = {hoSoBenhAn.TinhTrang},
                            @TrieuChung = {hoSoBenhAn.TrieuChung},
                            @TienSuBenh = {hoSoBenhAn.TienSuBenh},
                            @MaBacSi = {hoSoBenhAn.MaBacSi} 
                    """);
                    // Lưu ý: SP của bạn thiếu @MaBacSi, nhưng Bảng của bạn YÊU CẦU,
                    // tôi đã thêm @MaBacSi vào SP call (Bạn cần cập nhật SP hoặc Bảng)
                    // Tạm thời tôi sẽ giả định Bảng HoSoBenhAn cho phép MaBacSi null

                    TempData["Success"] = "Tạo hồ sơ bệnh án thành công!";
                    return RedirectToAction(nameof(Index), new { id = hoSoBenhAn.MaBenhNhan });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi tạo hồ sơ: " + ex.Message);
                }
            }
            ViewData["MaBacSi"] = new SelectList(_context.BacSis, "MaBacSi", "TenBacSi", hoSoBenhAn.MaBacSi);
            ViewData["MaBenhNhan"] = hoSoBenhAn.MaBenhNhan;
            return View(hoSoBenhAn);
        }

        // GET: HoSoBenhAn/ThemDieuTri/BN001
        public IActionResult ThemDieuTri(string maBenhNhan)
        {
            if (maBenhNhan == null) return NotFound();

            ViewData["MaBacSi"] = new SelectList(_context.BacSis, "MaBacSi", "TenBacSi");
            ViewData["MaBenhNhan"] = maBenhNhan;

            return View();
        }

        // POST: HoSoBenhAn/ThemDieuTri
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemDieuTri([Bind("MaDieuTri,MaBenhNhan,MaBacSi,NoiDung")] DieuTri dieuTri)
        {
            ModelState.Remove("MaBenhNhanNavigation");
            ModelState.Remove("MaBacSiNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi SP "sp_ThemDieuTri"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_ThemDieuTri
                            @MaDieuTri = {dieuTri.MaDieuTri},
                            @MaBenhNhan = {dieuTri.MaBenhNhan},
                            @MaBacSi = {dieuTri.MaBacSi},
                            @NoiDung = {dieuTri.NoiDung}
                    """);
                    TempData["Success"] = "Thêm thông tin điều trị thành công!";
                    return RedirectToAction(nameof(Index), new { id = dieuTri.MaBenhNhan });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm điều trị: " + ex.Message);
                }
            }
            ViewData["MaBacSi"] = new SelectList(_context.BacSis, "MaBacSi", "TenBacSi", dieuTri.MaBacSi);
            ViewData["MaBenhNhan"] = dieuTri.MaBenhNhan;
            return View(dieuTri);
        }
    }
}