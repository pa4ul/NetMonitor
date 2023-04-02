using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Dto;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using NetMonitor.Services;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.Pages.allHosts;

[Authorize(Roles = "Admin")]
public class Index : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly HostImportService _importService;

    public List<HostDto> Hosts = new List<HostDto>();
    [BindProperty] public HostDto Host { get; set; } = default!;
    [BindProperty] public IFormFile? UploadedFile { get; set; }
    private static string[] _allowedTextExtensions = { ".txt", ".csv" };
    private static string[] _allowedExcelExtensions = { ".xls", ".xlsx" };
    [TempData] public string? ErrorMessage { get; set; }
    [TempData] public string? Message { get; set; }


    public Index(NetMonitorContext db, HostImportService importService)
    {
        _db = db;
        _importService = importService;
    }

    public void OnGet()
    {
    }


    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid) return Page();
        var host = new Host(Host.Hostname, Host.IPAddress, new Description(Host.Description, Host.LongDescription));
        _db.Hosts.Add(host);
        _db.SaveChanges();
        return RedirectToPage();
    }

    public IActionResult OnPostDeleteHost(Guid guid)
    {
        var host = _db.Hosts.FirstOrDefault(h => h.Guid == guid);
        _db.Hosts.Remove(host);
        _db.SaveChanges();

        return RedirectToPage();
    }

    public IActionResult OnPostCsvImport()
    {
        var (success, message) = CheckUploadedFile(_allowedTextExtensions);
        if (!success)
        {
            ErrorMessage = message;
            return RedirectToPage();
        }

        using var stream = UploadedFile!.OpenReadStream();
        (success, message) = _importService.LoadCsv(stream);
        if (!success)
        {
            ErrorMessage = message;
        }
        else
        {
            Message = message;
        }

        return RedirectToPage();
    }

    public IActionResult OnPostExcelImport()
    {
        var (success, message) = CheckUploadedFile(_allowedExcelExtensions);
        if (!success)
        {
            ErrorMessage = message;
            return RedirectToPage();
        }

        using var stream = UploadedFile!.OpenReadStream();
        (success, message) = _importService.LoadExcel(stream);
        if (!success)
        {
            ErrorMessage = message;
        }
        else
        {
            Message = message;
        }

        return RedirectToPage();
    }

    public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        var hosts = _db.Hosts.Select(h =>
            new HostDto(h.Guid, h.Hostname, h.Description.description, h.Description.longdescription, h.IPAddress,
                new List<ServiceDto>())).ToList();
        Hosts = hosts;
    }

    private (bool success, string message) CheckUploadedFile(string[] allowedExtensions)
    {
        if (UploadedFile is null)
        {
            return (false, "Es wurde keine Datei hochgeladen.");
        }

        var extension = Path.GetExtension(UploadedFile.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            return (false,
                $"Es dürfen nur Dateien mit der Erweiterung {string.Join(",", allowedExtensions)} hochgeladen werden.");
        }

        if (UploadedFile.Length > 1 << 20) //1MB Größe
        {
            return (false, "Die Datei darf maximal 1 MB groß sein.");
        }

        return (true, string.Empty);
    }
}