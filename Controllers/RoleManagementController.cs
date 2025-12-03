using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    // YÊU CẦU QUYỀN "Admin" MỚI ĐƯỢC VÀO
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagementController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Trang chính: Liệt kê tất cả User và Role của họ
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var viewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                viewModel.Add(new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            return View(viewModel);
        }

        // GET: Màn hình quản lý Role cho 1 User
        public async Task<IActionResult> Manage(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var viewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                // Tạo danh sách (Checkbox/Dropdown) tất cả các Role
                RolesList = allRoles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userRoles.Contains(r.Name) // Đánh dấu các Role user đang có
                }).ToList(),
                SelectedRoles = userRoles.ToList()
            };

            return View(viewModel);
        }

        // POST: Cập nhật Role cho 1 User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Lấy danh sách Role được chọn từ form (SelectedRoles)
            // Nếu model.SelectedRoles là null (không chọn role nào), khởi tạo list rỗng
            var selectedRoles = model.SelectedRoles ?? new List<string>();

            // 1. Xóa các Role cũ mà không có trong danh sách mới
            var rolesToRemove = userRoles.Except(selectedRoles);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            // 2. Thêm các Role mới mà user chưa có
            var rolesToAdd = selectedRoles.Except(userRoles);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            TempData["Success"] = "Cập nhật quyền thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}