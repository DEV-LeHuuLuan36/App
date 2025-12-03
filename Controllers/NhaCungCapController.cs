using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class NhaCungCapController : Controller
    {
        private readonly QLBenhVienContext _context;

        public NhaCungCapController(QLBenhVienContext context)
        {
            _context = context;
        }

        // GET: NhaCungCap
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả, bao gồm cả NCC đã ẩn (TrangThai = 0)
            return View(await _context.NhaCungCaps.OrderBy(n => n.TenNcc).ToListAsync());
        }

        // GET: NhaCungCap/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaCungCap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNcc,TenNcc,DiaChi,SoDienThoai,Email,MaSoThue,NguoiDaiDien,TrangThai")] NhaCungCap nhaCungCap)
        {
            // Mặc định là Hoạt động khi tạo mới
            nhaCungCap.TrangThai = true;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(nhaCungCap);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Thêm Nhà cung cấp thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm: " + ex.Message);
                }
            }
            return View(nhaCungCap);
        }

        // GET: NhaCungCap/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null) return NotFound();
            return View(nhaCungCap);
        }

        // POST: NhaCungCap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNcc,TenNcc,DiaChi,SoDienThoai,Email,MaSoThue,NguoiDaiDien,TrangThai")] NhaCungCap nhaCungCap)
        {
            if (id != nhaCungCap.MaNcc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaCungCap);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật Nhà cung cấp thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            return View(nhaCungCap);
        }

        // POST: NhaCungCap/ToggleStatus/NCC01
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            if (id == null) return NotFound();

            var nhaCungCap = await _context.NhaCungCaps.FindAsync(id);
            if (nhaCungCap == null) return NotFound();

            try
            {
                // Lật ngược trạng thái
                nhaCungCap.TrangThai = !nhaCungCap.TrangThai;
                _context.Update(nhaCungCap);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật trạng thái NCC thành công.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}