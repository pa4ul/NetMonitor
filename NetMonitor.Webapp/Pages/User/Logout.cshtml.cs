using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMonitor.Webapp.Services;

namespace NetMonitor.Webapp.Pages.User;

public class Logout : PageModel
{
    private readonly AuthService _authService;

    public Logout(AuthService authService)
    {
        _authService = authService;
    }

    public async Task<IActionResult> OnGet()
    {
        await _authService.LogoutAsync();
        return Redirect("/");
    }
}