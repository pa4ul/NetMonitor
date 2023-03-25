using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class IndexModel : PageModel
{
    private readonly NetMonitorContext _db;
    public List<MonitorInstanceDto> MonitorInstances { get; private set; } = new();

    public IndexModel(NetMonitorContext db)
    {
        _db = db;
    }

    public IActionResult OnGet()
    {
        var monitorInstance = _db.MonitorInstances
            .Select(m => new MonitorInstanceDto(m.Name,
                m.Hosts.Select(h => new HostDto(h.Guid, h.Hostname, h.Description.description,
                    h.Description.longdescription, h.IPAddress, new List<ServiceDto>())).ToList(), m.Guid)).ToList();
        if (monitorInstance is null) return RedirectToPage("/");
        MonitorInstances = monitorInstance;
        return Page();
    }
}