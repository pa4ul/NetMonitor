using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using NetMonitor.Webapp.Services;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class IndexModel : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public string Username => _authService.Username;
    public bool IsAdmin => _authService.IsAdmin;
    public bool isAuthenticated => _authService.IsAuthenticated;
    public IndexModel(NetMonitorContext db, AuthService authService, IMapper mapper)
    {
        _db = db;
        _authService = authService;
        _mapper = mapper;
    }

    public List<MonitorInstanceDto> MonitorInstances { get; private set; } = new();

    public IActionResult OnGet()
    {
        var monitorInstance = _db.MonitorInstances
            .Select(m => new MonitorInstanceDto(m.Name,
                m.Hosts.Select(h => new HostDto(h.Guid, h.Hostname, h.Description.description,
                    h.Description.longdescription, h.IPAddress, new List<ServiceDto>())).ToList(), m.Guid, m.Manager)).ToList();

        
        if (monitorInstance is null) return RedirectToPage("/");
        MonitorInstances = monitorInstance;
        return Page();
    }
}