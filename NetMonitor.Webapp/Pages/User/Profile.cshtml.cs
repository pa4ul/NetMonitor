using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using NetMonitor.Webapp.Services;

namespace NetMonitor.Webapp.Pages.User;

public class Profile : PageModel
{
    private readonly NetMonitorContext _db;
    private readonly AuthService _authService;

    public Profile(NetMonitorContext db, AuthService authService)
    {
        _db = db;
        _authService = authService;
    }

    public Model.User User { get; set; }
    [TempData] public string Message { get; set; }
    [BindProperty] public string Username { get; set; }

    [BindProperty] public IFormFile? UploadedFile { get; set; }
    public string UploadedFileData { get; set; }
   
    
    public void OnGet()
    {
        User = _db.Users.FirstOrDefault(u => u.Username == _authService.Username);
        UploadedFileData = _db.Users.FirstOrDefault(u => u.Username == _authService.Username).ImageData;
    }

    public IActionResult OnPostImageImport()
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == _authService.Username);

        string[] allowedExtensions = { ".png", ".jpg", ".jpeg" };
        if (UploadedFile is null)
        {
        }

        var extension = Path.GetExtension(UploadedFile.FileName).ToLower();
        if (allowedExtensions.Contains(extension))
        {
            byte[] bytes = null;
            using (Stream fs = UploadedFile.OpenReadStream())
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    bytes = br.ReadBytes((Int32)fs.Length);
                }
            }

            UploadedFileData = Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        user.ImageData = UploadedFileData;
        _db.SaveChanges();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateUser()
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == _authService.Username);
        if (user is null)
        {
            Message = "Log out and try again";
            return RedirectToPage();
        }

        if (_db.Users.FirstOrDefault(u => u.Username == Username) is not null)
        {
            Message = "This Username already exists";
            return RedirectToPage();
        }


        user.Username = Username;
        _db.SaveChanges();
        await _authService.LogoutAsync();

        return Redirect("/User/Login");
    }
}