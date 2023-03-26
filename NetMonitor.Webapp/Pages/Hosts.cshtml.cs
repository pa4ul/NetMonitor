using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class Hosts : PageModel
{
    private readonly NetMonitorContext _db;

    public Hosts(NetMonitorContext db)
    {
        _db = db;
    }

    // We need the Guid in the post route, we can define a global property
    [FromRoute] public Guid Guid { get; set; }

    // NO BindProperty
    // We use the DTO for the direction Model --> Page
    public HostDto Host { get; private set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }

    /// <summary>
    /// /// Add a Cmd. This cmd is filled out by the modelbinder with the formdata.
    /// /// </summary>
    public record AddHostCmd(
        [StringLength(255, MinimumLength = 1)] string Hostname,
        [RegularExpression(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")] string IpAddress,
        [StringLength(255, MinimumLength = 1)] string Description,
        [MaxLength(255)] string? Longdescription = ""
        );

    /// <summary> /// Modelbinding read the values from the form for Host.
    /// /// Important: Use the same Prpoerties as in HostCmd!
    /// /// Use the debugger to check is model binding works.
    /// /// </summary>
    public IActionResult OnPostEdit([Bind(Prefix = "Host")] AddHostCmd cmd)
    {
        if (!ModelState.IsValid) return Page();
        var host = _db.Hosts.FirstOrDefault(h => h.Guid == Guid);
        if (host is null)
        {
            return RedirectToPage();
        }

        host.Hostname = cmd.Hostname;
        host.IPAddress = cmd.IpAddress;
        host.Description = new Model.Description(description: cmd.Description, longdescription: cmd.Longdescription);
        _db.SaveChanges();
        // Redirect after POST
        return RedirectToPage();
    }
    
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        // Hint: Define a ServiceDto without a messagelist and a ServiceDetailDto with a messagelist.
        var host = _db.Hosts.Where(h => h.Guid == Guid).Select(h => new HostDto(h.Guid, h.Hostname,
            h.Description.description, h.Description.longdescription, h.IPAddress,
            h.ServicesInUse.Select(s => new ServiceDto(s.Guid, s.Description.description, s.Description.longdescription,
                s.NormalInterval, s.RetryInterval, s.ServiceType, new List<MessageDto>())).ToList())).FirstOrDefault();
        if (host is null)
        {
            context.Result = Redirect("/");
            return;
        }

        Host = host;
    }
}