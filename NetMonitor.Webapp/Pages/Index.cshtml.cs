using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class IndexModel : PageModel
{
    private readonly NetMonitorContext _db;
    public List<MonitorInstance> MonitorInstances { get; private set; } = new();

    public IndexModel(NetMonitorContext db)
    {
        _db = db;
    }

    public void OnGet() 
    {
        MonitorInstances = _db.MonitorInstances
            .Include(h=>h.Hosts)
            .ToList();
    }
}