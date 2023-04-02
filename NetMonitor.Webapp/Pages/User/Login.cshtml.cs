using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Webapp.Services;

namespace NetMonitor.Webapp.Pages.User;

public class Login : PageModel
{
    private readonly AuthService _authService;
    private readonly IHostEnvironment _environment;
    public Login(AuthService authService, IHostEnvironment environment)
    {
        _authService = authService;
        _environment = environment;
    }
    public bool IsDevelopment => _environment.IsDevelopment();

    [BindProperty]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Ungültiger Username")]
    public string Username { get; set; } = default!;
    [BindProperty]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Ungültiges Passwort")]
    public string Password { get; set; } = default!;
    [FromQuery]
    public string? ReturnUrl { get; set; }
    public string? Message { get; private set; }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        } 
        var (success, message) = await _authService.TryLoginAsync(Username, Password);
        if (!success)
        {
            Message = message;
            return Page();
        }
        return Redirect(ReturnUrl ?? "/");
    }
}