using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Infrastructure;
using NetMonitor.Model;
using NetMonitor.Webapp.Services;

namespace NetMonitor.Webapp.Pages.User;

public class Register : PageModel
{
    private readonly AuthService _authService;
    private readonly IHostEnvironment _environment;
    private readonly ICryptService _cryptService;
    private readonly NetMonitorContext _db;

    public Register(AuthService authService, IHostEnvironment environment, ICryptService cryptService,
        NetMonitorContext db)
    {
        _authService = authService;
        _environment = environment;
        _cryptService = cryptService;
        _db = db;
    }

    public bool IsDevelopment => _environment.IsDevelopment();
    public string? Message { get; private set; }


    [BindProperty]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Ungültiger Username")]
    public string Username { get; set; } = default!;

    [BindProperty]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Ungültiges Passwort")]
    public string Password { get; set; } = default!;

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (_db.Users.Count(u=>u.Username==Username)>0)
        {
            Message = "Der Username existiert bereits";
            return Page();
        }
        
        var salt = _cryptService.GenerateSecret(256);
        var user = new Model.User(username: Username, salt: salt,
            passwordHash: _cryptService.GenerateHash(salt, Password), Usertype.Owner);
        _db.Users.Add(user);
        _db.SaveChanges();
        return RedirectToPage("/User/Login");
    }
}