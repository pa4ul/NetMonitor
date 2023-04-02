using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Infrastructure.Repositories;
using NetMonitor.Model;
using NetMonitor.Services;
using NetMonitor.Webapp.Services;
using NetMonitor.Webapp.wwwroot.Dto;

var opt = new DbContextOptionsBuilder()
    .UseSqlite("Data Source=NetMonitor.db") // Keep connection open (only needed with SQLite in memory db)
    .Options;
using (var db = new NetMonitorContext(opt))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed(new CryptService());
}

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<NetMonitorContext>(opt => { opt.UseSqlite("Data Source=NetMonitor.db"); });
builder.Services.AddTransient<HostImportService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddRazorPages();
builder.Services.AddTransient<UserRepository>();

// Services for authentication
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ICryptService, CryptService>();
builder.Services.AddTransient<AuthService>(provider => new AuthService(
    isDevelopment: builder.Environment.IsDevelopment(),
    db: provider.GetRequiredService<NetMonitorContext>(),
    crypt: provider.GetRequiredService<ICryptService>(),
    httpContextAccessor: provider.GetRequiredService<IHttpContextAccessor>()));
builder.Services.AddAuthentication(
        Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/User/Login";
        o.AccessDeniedPath = "/User/AccessDenied";
    });
builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("OwnerOrAdminRole", p => p.RequireRole(Usertype.Owner.ToString(), Usertype.Admin.ToString()));
});

// MIDDLEWARE
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();