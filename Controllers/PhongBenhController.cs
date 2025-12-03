using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class PhongBenhController : Controller
    {
        private readonly QLBenhVienContext _context;

        public PhongBenhController(QLBenhVienContext context)
        {
            _context = context;
        }

        private void LoadKhoaList()
        {
            ViewData["MaKhoa"] = new SelectList(_context.Khoas.Where(k => k.TrangThai == true), "MaKhoa", "TenKhoa");
        }

        public async Task<IActionResult> Index()
        {
            var phongBenhs = _context.PhongBenhs
                .Include(p => p.MaKhoaNavigation)
                .OrderBy(p => p.MaKhoaNavigation.TenKhoa)
                .ThenBy(p => p.TenPhong);

            return View(await phongBenhs.ToListAsync());
        }

        public IActionResult Create()
        {
            LoadKhoaList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhong,TenPhong,MaKhoa,SoGiuong,LoaiPhong,TrangThai")] PhongBenh phongBenh)
        {
            ModelState.Remove("MaKhoaNavigation");
            // Gán TrangThai = 1 (Active) khi tạo mới
            phongBenh.TrangThai = true;

            if (ModelState.IsValid)
            {
                try
                {
                    // SP sp_ThemPhongBenh của bạn thiếu nhiều trường, dùng EF Core
                    _context.Add(phongBenh);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm phòng bệnh thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            LoadKhoaList();
            return View(phongBenh);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var phongBenh = await _context.PhongBenhs.FindAsync(id);
            if (phongBenh == null) return NotFound();

            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", phongBenh.MaKhoa);
            return View(phongBenh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaPhong,TenPhong,MaKhoa,SoGiuong,LoaiPhong,TrangThai")] PhongBenh phongBenh)
        {
            if (id != phongBenh.MaPhong) return NotFound();

            ModelState.Remove("MaKhoaNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // SP sp_CapNhatPhongBenh của bạn thiếu nhiều trường
                    _context.Update(phongBenh);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật phòng bệnh thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            ViewData["MaKhoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", phongBenh.MaKhoa);
            return View(phongBenh);
        }

        // --- THÊM HÀM MỚI NÀY ---
        // POST: PhongBenh/ToggleStatus/P001
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            var phongBenh = await _context.PhongBenhs.FindAsync(id);
            if (phongBenh == null) return NotFound();

            try
            {
                // Lật ngược trạng thái
                phongBenh.TrangThai = !phongBenh.TrangThai;
                _context.Update(phongBenh);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái Phòng thành công.";
            }
            catch (Exception ex)
            {
                // Bắt lỗi nếu Phòng đang có Bệnh nhân
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // --- BỎ HÀM DELETE VÀ DELETECONFIRMED ---
    }
}