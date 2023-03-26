using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;

namespace NetMonitor.Webapp.Pages;

public class Services : PageModel
{
    private readonly NetMonitorContext _db;

    public Services(NetMonitorContext db)
    {
        _db = db;
    }

    public ServiceDto Service { get; private set; } = default!;

    public IActionResult OnGet(Guid guid)
    {
        var service = _db.Services
            .Where(s => s.Guid == guid)
            .Select(s => new ServiceDto(s.Guid, s.Description.description, s.Description.longdescription,
                s.NormalInterval, s.RetryInterval, s.ServiceType))
            .FirstOrDefault();
        if (service is null) return RedirectToPage("/");
        Service = service;
        return Page();
    }
}