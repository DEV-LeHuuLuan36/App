using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.ViewModels; // <-- Thêm ViewModel
using Microsoft.AspNetCore.Identity; // <-- Thêm Identity
using System.Data; // <-- Thêm thư viện DataTable
using Microsoft.Data.SqlClient; // <-- Thêm thư viện SQL

namespace App.Controllers
{
    [Authorize(Roles = "Role_DevLead_DevOps, Admin")]
    public class PhieuNhapThuocController : Controller
    {
        private readonly QLBenhVienContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration; // Dùng để lấy connection string

        public PhieuNhapThuocController(QLBenhVienContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        // GET: PhieuNhapThuoc
        public async Task<IActionResult> Index()
        {
            var phieuNhaps = _context.PhieuNhapThuocs
                .Include(p => p.MaNhanVienNavigation)
                .Include(p => p.MaNhaCungCapNavigation)
                .OrderByDescending(p => p.NgayNhap);
            return View(await phieuNhaps.ToListAsync());
        }

        // GET: PhieuNhapThuoc/Details/PN001
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var phieuNhap = await _context.PhieuNhapThuocs
                .Include(p => p.MaNhanVienNavigation)
                .Include(p => p.MaNhaCungCapNavigation)
                .FirstOrDefaultAsync(m => m.MaPhieuNhap == id);

            if (phieuNhap == null) return NotFound();

            // Lấy chi tiết phiếu nhập
            var chiTiet = await _context.ChiTietPhieuNhaps
                .Include(ct => ct.MaThuocNavigation)
                .Where(ct => ct.MaPhieuNhap == id)
                .ToListAsync();

            ViewBag.ChiTietPhieuNhap = chiTiet;
            return View(phieuNhap);
        }

        // GET: PhieuNhapThuoc/Create
        public IActionResult Create()
        {
            // Lấy MaNhanVien từ user đang đăng nhập (ví dụ)
            // Tạm thời ta sẽ load danh sách
            ViewData["MaNhanVien"] = new SelectList(_context.NhanViens.Where(n => n.TrangThai == true), "MaNhanVien", "HoTen");
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps.Where(n => n.TrangThai == true), "MaNcc", "TenNcc");
            ViewData["ThuocList"] = new SelectList(_context.Thuocs.Where(t => t.TrangThai == true), "MaThuoc", "TenThuoc");

            var viewModel = new PhieuNhapViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhieuNhapViewModel viewModel)
        {
            if (viewModel.ChiTietNhap == null || !viewModel.ChiTietNhap.Any())
            {
                ModelState.AddModelError("ChiTietNhap", "Phải có ít nhất 1 dòng chi tiết nhập thuốc.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["MaNhanVien"] = new SelectList(_context.NhanViens.Where(n => n.TrangThai == true), "MaNhanVien", "HoTen", viewModel.MaNhanVien);
                ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps.Where(n => n.TrangThai == true), "MaNcc", "TenNcc", viewModel.MaNhaCungCap);
                ViewData["ThuocList"] = new SelectList(_context.Thuocs, "MaThuoc", "TenThuoc");
                return View(viewModel);
            }
            var chiTietTable = new DataTable();
            chiTietTable.Columns.Add("MaCTPhieu", typeof(string));
            chiTietTable.Columns.Add("MaThuoc", typeof(string));
            chiTietTable.Columns.Add("SoLuong", typeof(int));
            chiTietTable.Columns.Add("DonGiaNhap", typeof(decimal));

            var hanSuDungColumn = new DataColumn("HanSuDung", typeof(DateTime));
            hanSuDungColumn.AllowDBNull = true; 
            chiTietTable.Columns.Add(hanSuDungColumn);

            foreach (var item in viewModel.ChiTietNhap)
            {
                chiTietTable.Rows.Add(
                    "CTN" + Guid.NewGuid().ToString().Substring(0, 7).ToUpper(),
                    item.MaThuoc,
                    item.SoLuong,
                    item.DonGiaNhap,
                    item.HanSuDung.HasValue
                        ? (object)item.HanSuDung.Value.ToDateTime(TimeOnly.MinValue)
                        : DBNull.Value
                );
            }

            var connectionString = _context.Database.GetConnectionString();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_NhapThuoc", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@MaPhieuNhap", viewModel.MaPhieuNhap);
                        command.Parameters.AddWithValue("@MaNhanVien", viewModel.MaNhanVien);
                        command.Parameters.AddWithValue("@MaNhaCungCap", (object)viewModel.MaNhaCungCap ?? DBNull.Value);

                        var tvpParam = command.Parameters.AddWithValue("@ChiTietNhap", chiTietTable);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.ChiTietNhapType";

                        await command.ExecuteNonQueryAsync();
                    }
                }

                TempData["Success"] = "Nhập thuốc thành công! (Đã dùng T-SQL UDTT)";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi gọi T-SQL: " + ex.Message);
            }
            ViewData["MaNhanVien"] = new SelectList(_context.NhanViens.Where(n => n.TrangThai == true), "MaNhanVien", "HoTen", viewModel.MaNhanVien);
            ViewData["MaNhaCungCap"] = new SelectList(_context.NhaCungCaps.Where(n => n.TrangThai == true), "MaNcc", "TenNcc", viewModel.MaNhaCungCap);
            ViewData["ThuocList"] = new SelectList(_context.Thuocs, "MaThuoc", "TenThuoc");

            return View(viewModel);
        }
    }
}