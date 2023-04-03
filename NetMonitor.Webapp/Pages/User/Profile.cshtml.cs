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

    public void OnGet()
    {
        User = _db.Users.FirstOrDefault(u => u.Username == _authService.Username);
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