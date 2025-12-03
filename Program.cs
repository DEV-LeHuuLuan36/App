using Microsoft.AspNetCore.Identity; // <- THÊM VÀO
using Microsoft.EntityFrameworkCore;
using App.Data; // <- THÊM VÀO (Chứa ApplicationDbContext)
using App.Models; // (Chứa QLBenhVienContext)
using App.Services; // (Chứa Service connection string động)

var builder = WebApplication.CreateBuilder(args);

// --- 1. ĐĂNG KÝ IDENTITY (CHO LOGIN/ROLES) ---

// Lấy connection string mặc định (dùng admin_luan)
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Đăng ký DbContext của Identity (ApplicationDbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(identityConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Thêm dịch vụ Identity và KÍCH HOẠT ROLES
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // <- RẤT QUAN TRỌNG: Kích hoạt Roles
    .AddEntityFrameworkStores<ApplicationDbContext>();


// --- 2. ĐĂNG KÝ DBCONTEXT NGHIỆP VỤ (QLBenhVienContext) ---

// Đăng ký các Service cho việc phân quyền động
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<IRoleBasedConnectionProvider, RoleBasedConnectionProvider>();

// Đăng ký QLBenhVienContext với connection string động
builder.Services.AddDbContext<QLBenhVienContext>((serviceProvider, options) =>
{
    var connectionProvider = serviceProvider.GetRequiredService<IRoleBasedConnectionProvider>();
    string connectionString = connectionProvider.GetConnectionString();
    options.UseSqlServer(connectionString);
});


// Thêm dịch vụ Controller và View
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- 3. KÍCH HOẠT MIDDLEWARE CỦA IDENTITY ---
// (Phải nằm giữa UseRouting và UseAuthorization)
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
// Thêm dòng này để các trang Login/Register (là Razor Pages) hoạt động
app.MapRazorPages(); 


// --- 4. TỰ ĐỘNG TẠO CÁC ROLE KHI CHẠY ---
// (Code này sẽ tạo các Role trong CSDL nếu chúng chưa tồn tại)
using (var scope = app.Services.CreateScope()) 
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Role_DevLead_DevOps", "Role_Developer", "Role_ReadOnly_Analyst", "Admin" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}


app.Run();