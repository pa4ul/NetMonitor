using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
    [BindProperty] public Dictionary<Guid, EditServiceCmd> EditServices { get; set; } = new();
    public List<ServiceDto> Services = default!;
    [BindProperty] public ServiceCmd Service { get; set; } = default!;

    public record EditServiceCmd(Guid Guid, int NormalInterval, int RetryInterval);
    
    public Index(NetMonitorContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public IActionResult OnGet()
    {
        EditServices = _db.Services.ProjectTo<EditServiceCmd>(_mapper.ConfigurationProvider)
            .ToDictionary(s => s.Guid, s => s);
        return Page();
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

    public IActionResult OnPostEditService(Guid serviceGuid)
    {
        var service = _db.Services.FirstOrDefault(s => s.Guid == serviceGuid);
        if (service is null) return RedirectToPage();
        _mapper.Map(EditServices[serviceGuid], service);
        _db.Entry(service).State = EntityState.Modified;
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