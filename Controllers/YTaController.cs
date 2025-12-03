using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class YTaController : Controller
    {
        private readonly QLBenhVienContext _context;

        public YTaController(QLBenhVienContext context)
        {
            _context = context;
        }

        private void LoadPhongList(string? selectedValue = null)
        {
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs.Where(p => p.TrangThai == true), "MaPhong", "TenPhong", selectedValue);
        }

        public async Task<IActionResult> Index()
        {
            var yTaList = _context.Yta
                .Include(y => y.MaPhongNavigation)
                .ThenInclude(p => p.MaKhoaNavigation)
                .OrderBy(y => y.TenYTa);

            return View(await yTaList.ToListAsync());
        }

        public IActionResult Create()
        {
            LoadPhongList();
            // CẢNH BÁO: KHÔNG NÊN DÙNG HÀM NÀY.
            // DÙNG NhanVien/Create VỚI LoaiNhanVien = "Y tá"
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaYTa,TenYTa,MaPhong,TrinhDo,SoDienThoai,Email,CCCD,TrangThai")] YTa yTa)
        {
            ModelState.Remove("MaPhongNavigation");
            yTa.TrangThai = true; // Mặc định

            if (ModelState.IsValid)
            {
                try
                {
                    // CẢNH BÁO: LOGIC NÀY BỊ LỖI KẾ THỪA
                    // Y TÁ NÀY SẼ KHÔNG HIỆN TRONG BẢNG LichLamViec
                    _context.Add(yTa);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Thêm y tá thành công! (CẢNH BÁO: Y tá này sẽ không hiển thị trong Lịch Làm Việc)";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            LoadPhongList(yTa.MaPhong);
            return View(yTa);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            // --- SỬA LỖI 404 (DO CHAR(10)) ---
            var yTa = await _context.Yta
                .FirstOrDefaultAsync(y => y.MaYTa.Trim() == id.Trim());
            // --- HẾT SỬA ---

            if (yTa == null) return NotFound();

            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs.Where(p => p.TrangThai == true), "MaPhong", "TenPhong", yTa.MaPhong);
            return View(yTa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaYTa,TenYTa,MaPhong,TrinhDo,SoDienThoai,Email,CCCD,TrangThai,KinhNghiem")] YTa yTa)
        {
            // --- SỬA LỖI 404 (DO CHAR(10)) ---
            if (id.Trim() != yTa.MaYTa.Trim()) return NotFound();
            // --- HẾT SỬA ---

            ModelState.Remove("MaPhongNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // CẢNH BÁO: LOGIC NÀY BỊ LỖI KẾ THỪA
                    _context.Update(yTa);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Cập nhật y tá thành công! (CẢNH BÁO: Logic Kế thừa có thể bị lỗi)";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            ViewData["MaPhong"] = new SelectList(_context.PhongBenhs.Where(p => p.TrangThai == true), "MaPhong", "TenPhong", yTa.MaPhong);
            return View(yTa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            // --- SỬA LỖI 404 (DO CHAR(10)) ---
            var yTa = await _context.Yta
                .FirstOrDefaultAsync(y => y.MaYTa.Trim() == id.Trim());
            // --- HẾT SỬA ---

            if (yTa == null) return NotFound();

            try
            {
                // CẢNH BÁO: LOGIC NÀY BỊ LỖI KẾ THỪA
                yTa.TrangThai = !yTa.TrangThai;
                _context.Update(yTa);

                // ĐỒNG BỘ BẢNG NHANVIEN (CHA)
                var nhanVien = await _context.NhanViens.FindAsync(id);
                if (nhanVien != null)
                {
                    nhanVien.TrangThai = yTa.TrangThai;
                    _context.Update(nhanVien);
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái Y tá thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}