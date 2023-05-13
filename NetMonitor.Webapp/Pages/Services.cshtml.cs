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
        var service = _db.Services
            .Where(s => s.Guid == guid)
            .Select(s => new ServiceDto(s.Guid, s.Description.description, s.Description.longdescription,
                s.NormalInterval, s.RetryInterval, s.ServiceType,
                s.Messages.Select(m => new MessageDto(m.Description.description, m.Description.longdescription, m.Date,m.MessageType))
                .ToList()))
            .FirstOrDefault();

        var service2 = _db.Services.FirstOrDefault(s => s.Guid == guid);
        Service = _mapper.Map<ServiceDto>(service2);
        var service3 = _db.Services.Select(s=>s).Where(s=>s.Guid==guid);

        var test = _mapper.ProjectTo<ServiceDto>(service3);
        
        if (service is null) return RedirectToPage("/");
        Service = service;
        return Page();
    }
}