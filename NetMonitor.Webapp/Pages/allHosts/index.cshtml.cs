using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages.allHosts;

public class index : PageModel
{
    private readonly NetMonitorContext _db;
    public List<HostDto> Hosts = new List<HostDto>();

    [BindProperty] public HostDto Host { get; set; } = default!;

    public index(NetMonitorContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
    }


    public IActionResult OnPostEdit()
    {
        Host host = new Host(Host.Hostname, Host.IPAddress, new Description(Host.Description, Host.LongDescription));
        _db.Hosts.Add(host);
        _db.SaveChanges();
        return RedirectToPage();
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var hosts = _db.Hosts.Select(h =>
            new HostDto(h.Guid, h.Hostname, h.Description.description, h.Description.longdescription, h.IPAddress,
                new List<ServiceDto>())).ToList();
        Hosts = hosts;
    }
}