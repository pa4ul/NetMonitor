using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class Hosts : PageModel
{
    private readonly NetMonitorContext _db;

    public Hosts(NetMonitorContext db)
    {
        _db = db;
    }

    public Host Host { get; private set; } = default!;

    public IActionResult OnGet(Guid guid)
    {
        var host = _db.Hosts
            .Include(h => h.ServicesInUse)
            .FirstOrDefault(h => h.Guid == guid);
        if (host is null) return RedirectToPage("/");
        Host = host;
        return Page();
    }
}