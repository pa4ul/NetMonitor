using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Webapp.Dto;

namespace NetMonitor.Webapp.Pages;

public class Services : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly IMapper _mapper;

    public Services(NetMonitorContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public ServiceDto Service { get; private set; } = default!;


    public IActionResult OnGet(Guid guid)
    {
        var service3 = _db.Services.Select(s => s).Where(s => s.Guid == guid);

        var serviceDto = _mapper.ProjectTo<ServiceDto>(service3);

        if (serviceDto is null) return RedirectToPage("/");
        Service = serviceDto.FirstOrDefault();
        return Page();
    }
}