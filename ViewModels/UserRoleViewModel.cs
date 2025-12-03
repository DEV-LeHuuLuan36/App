using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        // Danh sách tất cả các role
        public List<SelectListItem> RolesList { get; set; }
        // Danh sách các role user đang có
        public List<string> SelectedRoles { get; set; }
    }
}