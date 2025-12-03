using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // <- Thư viện đã có

namespace App.Controllers
{
    // Module này chỉ dành cho Developer, DevLead và Admin
    [Authorize(Roles = "Role_Developer, Role_DevLead_DevOps, Admin")]
    public class BenhNhanController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BenhNhanController(QLBenhVienContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: BenhNhan
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin user (code này giờ đã an toàn)
            string tenUser = User.Identity.Name;
            string userId = _userManager.GetUserId(User);
            IdentityUser userHienTai = await _userManager.GetUserAsync(User);

            var benhNhans = _context.BenhNhans
                .Include(b => b.MaPhongNavigation)
                .OrderByDescending(b => b.NgayNhapVien); // Sắp xếp cho đẹp

            return View(await benhNhans.ToListAsync());
        }

        // GET: BenhNhan/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var benhNhan = await _context.BenhNhans
                .Include(b => b.MaPhongNavigation)
                .FirstOrDefaultAsync(m => m.MaBenhNhan == id);
            if (benhNhan == null) return NotFound();
            return View(benhNhan);
        }

        // GET: BenhNhan/Create
        public IActionResult Create()
        {
            // Gửi danh sách phòng bệnh qua View
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong");
            return View();
        }

        // POST: BenhNhan/Create (Code này đã TỐT, giữ nguyên)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBenhNhan,HoTenBenhNhan,NgaySinh,GioiTinh,DiaChi,SoDienThoai,Email,MaPhong")] BenhNhan benhNhan)
        {
            ModelState.Remove("NgayNhapVien");
            ModelState.Remove("TrangThai");
            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_ThemBenhNhan
                    @MaBenhNhan = {benhNhan.MaBenhNhan},
                    @HoTenBenhNhan = {benhNhan.HoTenBenhNhan}, 
                    @NgaySinh = {benhNhan.NgaySinh},
                    @GioiTinh = {benhNhan.GioiTinh},
                    @DiaChi = {benhNhan.DiaChi},
                    @MaPhong = {benhNhan.MaPhong},
                    @SoDienThoai = {benhNhan.SoDienThoai},
                    @Email = {benhNhan.Email}
                    """);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm bệnh nhân: " + ex.Message);
                }
            }
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong", benhNhan.MaPhong);
            return View(benhNhan);
        }

        // GET: BenhNhan/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var benhNhan = await _context.BenhNhans.FindAsync(id);
            if (benhNhan == null) return NotFound();
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong", benhNhan.MaPhong);
            return View(benhNhan);
        }

        // POST: BenhNhan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaBenhNhan,HoTenBenhNhan,NgaySinh,GioiTinh,DiaChi,SoDienThoai,Email,MaPhong,NgayNhapVien,TrangThai")] BenhNhan benhNhan)
        {
            if (id != benhNhan.MaBenhNhan) return NotFound();

            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Gọi SP "sp_CapNhatBenhNhan"
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_CapNhatBenhNhan
                    @MaBenhNhan = {benhNhan.MaBenhNhan},
                    @HoTen = {benhNhan.HoTenBenhNhan}, 
                    @NgaySinh = {benhNhan.NgaySinh},
                    @GioiTinh = {benhNhan.GioiTinh},
                    @DiaChi = {benhNhan.DiaChi},
                    @SoDienThoai = {benhNhan.SoDienThoai},
                    @Email = {benhNhan.Email}
                    """);

                    // 2. Cập nhật MaPhong (code này đã đúng, giữ nguyên)
                    var benhNhanTuDb = await _context.BenhNhans.FindAsync(id);
                    if (benhNhanTuDb.MaPhong != benhNhan.MaPhong)
                    {
                        benhNhanTuDb.MaPhong = benhNhan.MaPhong;
                        _context.Update(benhNhanTuDb);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs, "MaPhong", "TenPhong", benhNhan.MaPhong);
            return View(benhNhan);
        }

        // GET: BenhNhan/XuatVien/5 (Code này đã TỐT, giữ nguyên)
        public async Task<IActionResult> XuatVien(string id)
        {
            if (id == null) return NotFound();
            var benhNhan = await _context.BenhNhans
                .Include(b => b.MaPhongNavigation)
                .FirstOrDefaultAsync(m => m.MaBenhNhan == id);
            if (benhNhan == null) return NotFound();

            if (benhNhan.NgayXuatVien != null)
            {
                TempData["Error"] = "Bệnh nhân này đã xuất viện!";
                return RedirectToAction(nameof(Index));
            }

            return View(benhNhan);
        }

        // POST: BenhNhan/XuatVien/5 (Code này đã TỐT, giữ nguyên)
        [HttpPost, ActionName("XuatVien")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XuatVienConfirmed(string id)
        {
            try
            {
                // Gọi SP "sp_XuatVienBenhNhan"
                await _context.Database.ExecuteSqlInterpolatedAsync($"""
                    EXEC sp_XuatVienBenhNhan @MaBenhNhan = {id}
                """);
                TempData["Success"] = "Xuất viện cho bệnh nhân thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xuất viện: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}