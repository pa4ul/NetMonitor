using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Infrastructure.Repositories;
using NetMonitor.Model;
using NetMonitor.Services;
using NetMonitor.Webapp.Services;
using NetMonitor.Webapp.wwwroot.Dto;


var builder = WebApplication.CreateBuilder(args);

var opt = builder.Environment.IsDevelopment()
    ? new DbContextOptionsBuilder().UseSqlite(builder.Configuration.GetConnectionString("Sqlite"))
        .EnableSensitiveDataLogging().Options
    : new DbContextOptionsBuilder().UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")).Options;

using (var db = new NetMonitorContext(opt))
{
    if (builder.Environment.IsDevelopment())
    {
        db.Database.EnsureDeleted();
    }

    // Creating the tables when the database is empty or not present. 
    if (db.Database.EnsureCreated()) // Initialize only 1 time.
    {
        db.Initialize(
            new CryptService(),
            Environment.GetEnvironmentVariable("STORE_ADMIN") ??
            throw new ArgumentNullException("Die Variable STORE_ADMIN ist nicht gesetzt."));
    }

    if (builder.Environment.IsDevelopment())
    {
        db.Seed(new CryptService());
    }
}

// Add services to the container.
builder.Services.AddDbContext<NetMonitorContext>(opt =>
{
    if (builder.Environment.IsDevelopment())
        opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")).EnableSensitiveDataLogging();
    else
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
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
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} 
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// https://blog.elmah.io/the-asp-net-core-security-headers-guide/
/*app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
    // https://wiki.selfhtml.org/wiki/Sicherheit/Content_Security_Policy
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' data:");
    await next();
});*/

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();