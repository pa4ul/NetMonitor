using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure.Repositories;

namespace NetMonitor.Webapp.Pages.User;

public class Index : PageModel
{
    private readonly UserRepository _users;

    public Index(UserRepository users)
    {
        _users = users;
    }

    public IEnumerable<Model.User> Users =>
        _users.Set
            .Include(u => u.MonitorInstances)
            .OrderBy(u => u.Usertype).ThenBy(u => u.Username);
    public void OnGet()
    {

    }
}