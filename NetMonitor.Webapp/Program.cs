
// Erstellen und seeden der Datenbank

using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;

var opt = new DbContextOptionsBuilder()
    .UseSqlite("Data Source=NetMonitor.db")  // Keep connection open (only needed with SQLite in memory db)
    .Options;
using (var db = new NetMonitorContext(opt))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Seed();
}

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<NetMonitorContext>(opt =>
{
    opt.UseSqlite("Data Source=stores.db");
});
builder.Services.AddRazorPages();

// MIDDLEWARE
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();