using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetMonitor.Model;
using NetMonitor.Webapp.Dto;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class Hosts : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly IMapper _mapper;

    public Hosts(NetMonitorContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    // We need the Guid in the post route, we can define a global property
    [FromRoute] public Guid Guid { get; set; }

    // NO BindProperty
    // We use the DTO for the direction Model --> Page
    public HostDto Host { get; private set; } = default!;

    public IEnumerable<SelectListItem> ServiceSelectedList => _db.Services
        .OrderBy(s => s.Description.description)
        .Select(s => new SelectListItem(s.Description.description, s.Guid.ToString()));

    [BindProperty] public Guid ServiceGuid { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    /// <summary>
    /// /// Add a Cmd. This cmd is filled out by the modelbinder with the formdata.
    /// /// </summary>
    public record AddHostCmd(
        [StringLength(255, MinimumLength = 1)] string Hostname,
        [RegularExpression(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")]
        string IpAddress,
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
    public IActionResult OnPostDelete()
    {
        var host = _db.Hosts.FirstOrDefault(h => h.Guid == Guid);
        var messages = _db.Messages.Where(m => m.Host.Guid == Guid).ToList();
        var services = _db.Services.Where(s => s.Host.Guid == Guid).ToList();
        _db.Messages.RemoveRange(messages);
        _db.Services.RemoveRange(services);

        _db.SaveChanges();
        _db.Hosts.Remove(host);
        try
        {
            _db.SaveChanges();

        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e.InnerException?.Message);
        }
        return RedirectToPage();
    }
    public IActionResult OnPostRemoveAllServices()
    {
        var host = _db.Hosts.Include(h => h.ServicesInUse).FirstOrDefault(h => h.Guid == Guid);
        host.RemoveAllServices();
        _db.SaveChanges();

        return RedirectToPage();
    }
    public IActionResult OnPostAssignService()
    {
        var service = _db.Services.FirstOrDefault(s => s.Guid == ServiceGuid);
        if (service is null) return RedirectToPage();
        var host = _db.Hosts.FirstOrDefault(h => h.Guid == Guid);
        if (host is null) return RedirectToPage();
        host.AddService(service);
        _db.SaveChanges();

        return RedirectToPage();
    }
    public IActionResult OnPostRemoveService(Guid guid)
    {
        var service = _db.Services.FirstOrDefault(s => s.Guid == guid);
        var host = _db.Hosts
            .FirstOrDefault(h => h.Guid == Guid);
        host.RemoveService(service);
        _db.SaveChanges();
        return RedirectToPage();
    }
    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
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