using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Dto;
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

    public HostDto Host { get; private set; } = default!;

    public IActionResult OnGet(Guid guid)
    {
        var host = _db.Hosts
            .Where(h => h.Guid == guid)
            .Select(h => new HostDto(h.Guid, h.Hostname, h.Description.description, h.Description.longdescription, h.IPAddress,
                h.ServicesInUse.Select(s => new ServiceDto(s.Guid, s.Description.description, s.Description.longdescription,
                    s.NormalInterval, s.RetryInterval, s.ServiceType)).ToList()
            ))
            .FirstOrDefault();

        if (host is null) return RedirectToPage("/");
        Host = host;
        return Page();
    }
}