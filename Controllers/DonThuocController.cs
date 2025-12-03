using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.ViewModels; // <-- Thêm ViewModel

namespace App.Controllers
{
    [Authorize(Roles = "Role_Developer, Role_DevLead_DevOps, Admin")]
    public class DonThuocController : Controller
    {
        private readonly QLBenhVienContext _context;

        public DonThuocController(QLBenhVienContext context)
        {
            _context = context;
        }

        // GET: DonThuoc/Index/[MaDieuTri]
        // Trang chính: Hiển thị đơn thuốc của 1 lần điều trị
        public async Task<IActionResult> Index(string id) // id = MaDieuTri
        {
            if (id == null) return NotFound();

            var dieuTri = await _context.DieuTris
                .Include(d => d.MaBenhNhanNavigation)
                .Include(d => d.MaBacSiNavigation)
                .FirstOrDefaultAsync(d => d.MaDieuTri == id);

            if (dieuTri == null) return NotFound();

            var danhSachDonThuoc = await _context.DonThuocs
                .Include(dt => dt.MaThuocNavigation) // Lấy tên thuốc
                .Where(dt => dt.MaDieuTri == id)
                .ToListAsync();

            var viewModel = new DonThuocViewModel
            {
                DieuTri = dieuTri,
                DanhSachDonThuoc = danhSachDonThuoc
            };

            return View(viewModel);
        }

        // GET: DonThuoc/Create/[MaDieuTri]
        public IActionResult Create(string id) // id = MaDieuTri
        {
            if (id == null) return NotFound();

            ViewData["MaThuoc"] = new SelectList(_context.Thuocs.Where(t => t.SoLuongTon > 0 && t.TrangThai == true), "MaThuoc", "TenThuoc");

            var model = new DonThuoc { MaDieuTri = id }; // Tạo model mới và gán sẵn MaDieuTri
            return View(model);
        }

        // POST: DonThuoc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaDonThuoc,MaDieuTri,MaThuoc,SoLuong,CachDung")] DonThuoc donThuoc)
        {
            ModelState.Remove("MaBenhNhan");
            ModelState.Remove("MaBacSi");
            ModelState.Remove("MaBenhNhanNavigation");
            ModelState.Remove("MaBacSiNavigation");
            ModelState.Remove("MaThuocNavigation");
            ModelState.Remove("MaDieuTriNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Gọi SP "sp_KeDonThuoc" (đã sửa)
                    await _context.Database.ExecuteSqlInterpolatedAsync($"""
                        EXEC sp_KeDonThuoc
                            @MaDonThuoc = {donThuoc.MaDonThuoc},
                            @MaDieuTri = {donThuoc.MaDieuTri},
                            @MaThuoc = {donThuoc.MaThuoc},
                            @SoLuong = {donThuoc.SoLuong},
                            @CachDung = {donThuoc.CachDung}
                    """);
                    TempData["Success"] = "Kê đơn thuốc thành công!";
                    return RedirectToAction(nameof(Index), new { id = donThuoc.MaDieuTri });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi kê đơn: " + ex.Message);
                }
            }
            ViewData["MaThuoc"] = new SelectList(_context.Thuocs.Where(t => t.SoLuongTon > 0 && t.TrangThai == true), "MaThuoc", "TenThuoc", donThuoc.MaThuoc);
            return View(donThuoc);
        }
    }
}