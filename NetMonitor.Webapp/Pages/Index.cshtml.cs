using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Infrastructure.Repositories;
using NetMonitor.Model;
using NetMonitor.Webapp.Dto;
using NetMonitor.Webapp.Services;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages;

public class IndexModel : PageModel
{
    private readonly InstanceRepository _instances;
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public string Username => _authService.Username;
    public bool IsAdmin => _authService.IsAdmin;
    public bool isAuthenticated => _authService.IsAuthenticated;
    public IndexModel(InstanceRepository instances, AuthService authService, IMapper mapper)
    {
        _instances = instances;
        _authService = authService;
        _mapper = mapper;
    }

    public List<InstanceRepository.MonitorInstanceDto> MonitorInstances { get; private set; } = new();

    public IActionResult OnGet()
    {
        var monitorInstance = _instances.GetMonitorInstanceDtos();
        
        if (monitorInstance is null) return RedirectToPage("/");
        MonitorInstances = monitorInstance;
        return Page();
    }
}