using System.Security.Claims;

namespace App.Services
{
    public class RoleBasedConnectionProvider : IRoleBasedConnectionProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _defaultConnectionString;

        public RoleBasedConnectionProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public string GetConnectionString()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Nếu không có user đăng nhập (ví dụ: đang ở trang Login), dùng quyền admin (default)
            if (httpContext == null || httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return _defaultConnectionString;
            }

            var user = httpContext.User;

            // Ưu tiên Role cao nhất
            if (user.IsInRole("Role_DevLead_DevOps"))
            {
                return _configuration.GetConnectionString("Role_DevLead_DevOps");
            }

            if (user.IsInRole("Role_Developer"))
            {
                return _configuration.GetConnectionString("Role_Developer");
            }

            if (user.IsInRole("Role_ReadOnly_Analyst"))
            {
                return _configuration.GetConnectionString("Role_ReadOnly_Analyst");
            }

            // Mặc định trả về quyền admin nếu là user 'admin_luan'
            // (Bạn cần seed user 'admin_luan' vào ASP.NET Identity và gán role 'Admin' cho nó)
            if (user.IsInRole("Admin"))
            {
                return _defaultConnectionString;
            }

            // Nếu user đã đăng nhập nhưng không có Role phù hợp,
            // trả về connection string có quyền thấp nhất (ReadOnly) để đảm bảo an toàn.
            return _configuration.GetConnectionString("Role_ReadOnly_Analyst");
        }
    }
}