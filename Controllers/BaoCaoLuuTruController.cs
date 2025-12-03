using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Cần để lấy nhân viên

namespace App.Controllers
{
    // Chỉ DevLead và Admin mới được quản lý
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class BaoCaoLuuTruController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BaoCaoLuuTruController(QLBenhVienContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Hàm helper tải danh sách Nhân viên
        private void LoadNhanVienList(string? selectedTao = null, string? selectedDuyet = null)
        {
            var nhanVienList = _context.NhanViens.Where(n => n.TrangThai == true).ToList();
            ViewData["NguoiTao"] = new SelectList(nhanVienList, "MaNhanVien", "HoTen", selectedTao);
            ViewData["NguoiDuyet"] = new SelectList(nhanVienList, "MaNhanVien", "HoTen", selectedDuyet);
        }

        // GET: BaoCaoLuuTru
        public async Task<IActionResult> Index()
        {
            var baoCaos = _context.BaoCaos
                .Include(b => b.NguoiDuyetNavigation)
                .Include(b => b.NguoiTaoNavigation)
                .OrderByDescending(b => b.NgayTao);

            return View(await baoCaos.ToListAsync());
        }

        // GET: BaoCaoLuuTru/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var baoCao = await _context.BaoCaos
                .Include(b => b.NguoiDuyetNavigation)
                .Include(b => b.NguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaBaoCao == id);
            if (baoCao == null) return NotFound();
            return View(baoCao);
        }

        // GET: BaoCaoLuuTru/Create
        public IActionResult Create()
        {
            // Tự động gán Người tạo (ví dụ)
            // TODO: Cần logic map User ID sang MaNhanVien
            // string maNhanVien = ... 
            LoadNhanVienList();
            return View();
        }

        // POST: BaoCaoLuuTru/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaBaoCao,LoaiBaoCao,TuNgay,DenNgay,NoiDung,NguoiTao,TrangThai")] BaoCao baoCao)
        {
            ModelState.Remove("NguoiTaoNavigation");
            ModelState.Remove("NguoiDuyetNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(baoCao);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo báo cáo lưu trữ thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            LoadNhanVienList(baoCao.NguoiTao);
            return View(baoCao);
        }

        // GET: BaoCaoLuuTru/Edit/5 (Dùng để Duyệt báo cáo)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var baoCao = await _context.BaoCaos.FindAsync(id);
            if (baoCao == null) return NotFound();

            LoadNhanVienList(baoCao.NguoiTao, baoCao.NguoiDuyet);
            return View(baoCao);
        }

        // POST: BaoCaoLuuTru/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaBaoCao,LoaiBaoCao,TuNgay,DenNgay,NoiDung,NgayTao,NguoiTao,TrangThai,NguoiDuyet,NgayDuyet")] BaoCao baoCao)
        {
            if (id != baoCao.MaBaoCao) return NotFound();

            ModelState.Remove("NguoiTaoNavigation");
            ModelState.Remove("NguoiDuyetNavigation");

            if (ModelState.IsValid)
            {
                try
                {
                    // Nếu duyệt, tự động gán ngày duyệt
                    if (baoCao.TrangThai == "Đã duyệt" || baoCao.TrangThai == "Từ chối")
                    {
                        baoCao.NgayDuyet = DateTime.Now;
                        // TODO: Gán NguoiDuyet = Mã nhân viên của Admin đang đăng nhập
                    }

                    _context.Update(baoCao);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật báo cáo thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            LoadNhanVienList(baoCao.NguoiTao, baoCao.NguoiDuyet);
            return View(baoCao);
        }

        // GET: BaoCaoLuuTru/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var baoCao = await _context.BaoCaos
                .Include(b => b.NguoiTaoNavigation)
                .FirstOrDefaultAsync(m => m.MaBaoCao == id);
            if (baoCao == null) return NotFound();
            return View(baoCao);
        }

        // POST: BaoCaoLuuTru/Delete/5 (Hard Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var baoCao = await _context.BaoCaos.FindAsync(id);
                if (baoCao != null)
                {
                    _context.BaoCaos.Remove(baoCao);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa báo cáo lưu trữ thành công.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}