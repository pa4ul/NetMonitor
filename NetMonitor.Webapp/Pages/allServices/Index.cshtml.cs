using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using NetMonitor.Webapp.Dto;

namespace NetMonitor.Webapp.Pages.allServices;
[Authorize(Roles = "Admin")]
public class Index : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly IMapper _mapper;
    public List<ServiceDto> Services = default!;
    [BindProperty] public ServiceCmd Service { get; set; } = default!;

    public Index(NetMonitorContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid) return Page();
        var service = new Service(null, Service.NormalInterval, Service.RetryInterval,
            new Description(Service.Description, Service.LongDescription));
        _db.Services.Add(service);
        _db.SaveChanges();
        return RedirectToPage();
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var services = _db.Services.Select(s => new ServiceDto(s.Guid, s.Description.description,
            s.Description.longdescription, s.NormalInterval, s.RetryInterval, s.ServiceType,
            new List<MessageDto>())).ToList();

        Services = services;
    }
}