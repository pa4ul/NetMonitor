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
using NetMonitor.Infrastructure.Repositories;
using NetMonitor.Model;
using NetMonitor.Webapp.Dto;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class Hosts : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly IMapper _mapper;
    private readonly HostRepository _repository;

    public Hosts(NetMonitorContext db, IMapper mapper, HostRepository repository)
    {
        _db = db;
        _mapper = mapper;
        _repository = repository;
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

    /// <summary> /// Modelbinding read the values from the form for Host.
    /// /// Important: Use the same Prpoerties as in HostCmd!
    /// /// Use the debugger to check is model binding works.
    /// /// </summary>
    public IActionResult OnPostEdit([Bind(Prefix = "Host")] NetMonitor.Infrastructure.Repositories.HostRepository.AddHostCmd cmd)
    {
        if (!ModelState.IsValid) return Page();
        _repository.Edit(cmd,Guid);
        return RedirectToPage();
    }
    public IActionResult OnPostDelete()
    {
        _repository.Delete(Guid);
        return RedirectToPage();
    }
    public IActionResult OnPostRemoveAllServices()
    {
        _repository.RemoveAllServices(Guid);
        return RedirectToPage();
    }
    public IActionResult OnPostAssignService()
    {
        _repository.AssignService(ServiceGuid,Guid);
        return RedirectToPage();
    }
    public IActionResult OnPostRemoveService(Guid guid)
    {
        _repository.RemoveService(guid,Guid);
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