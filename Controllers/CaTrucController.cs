using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class CaTrucController : Controller
    {
        private readonly QLBenhVienContext _context;

        public CaTrucController(QLBenhVienContext context)
        {
            _context = context;
        }

        // GET: CaTruc
        public async Task<IActionResult> Index()
        {
            return View(await _context.CaTrucs.OrderBy(c => c.GioBatDau).ToListAsync());
        }

        // GET: CaTruc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CaTruc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCa,TenCa,GioBatDau,GioKetThuc,HeSoLuong,MoTa")] CaTruc caTruc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(caTruc);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo ca trực mới thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    string errorMessage = ex.Message;
                    // Lấy lỗi chi tiết bên trong (InnerException)
                    if (ex.InnerException != null)
                    {
                        errorMessage = ex.InnerException.Message;
                    }

                    // Kiểm tra các lỗi CSDL phổ biến
                    if (errorMessage.Contains("PRIMARY KEY constraint"))
                    {
                        ModelState.AddModelError("MaCaTruc", $"Mã ca trực '{caTruc.MaCa}' đã tồn tại.");
                    }
                    else if (errorMessage.Contains("FOREIGN KEY constraint"))
                    {
                        if (errorMessage.Contains("MaNhanVien"))
                        {
                            ModelState.AddModelError("MaNhanVien", "Mã nhân viên bạn chọn không tồn tại.");
                        }
                        else if (errorMessage.Contains("MaCa"))
                        {
                            ModelState.AddModelError("MaCa", "Mã ca bạn chọn không tồn tại.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Lỗi Khóa Ngoại: Dữ liệu bạn chọn không hợp lệ.");
                        }
                    }
                    else if (errorMessage.Contains("UNIQUE KEY constraint"))
                    {
                        ModelState.AddModelError("", "Lỗi Trùng lặp: Dữ liệu này đã tồn tại (ví dụ: CCCD, Email).");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Lỗi CSDL: " + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    // Bắt các lỗi chung khác
                    ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                }
            }
            return View(caTruc);
        }

        // GET: CaTruc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var caTruc = await _context.CaTrucs.FindAsync(id);
            if (caTruc == null) return NotFound();
            return View(caTruc);
        }

        // POST: CaTruc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaCa,TenCa,GioBatDau,GioKetThuc,HeSoLuong,MoTa")] CaTruc caTruc)
        {
            if (id != caTruc.MaCa) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caTruc);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật ca trực thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }
            return View(caTruc);
        }

        // GET: CaTruc/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var caTruc = await _context.CaTrucs
                .FirstOrDefaultAsync(m => m.MaCa == id);
            if (caTruc == null) return NotFound();
            return View(caTruc);
        }

        // POST: CaTruc/Delete/5 (Hard Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var caTruc = await _context.CaTrucs.FindAsync(id);
                if (caTruc != null)
                {
                    _context.CaTrucs.Remove(caTruc);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Xóa ca trực thành công.";
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