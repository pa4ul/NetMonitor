using NetMonitor.Model;

namespace NetMonitor.Infrastructure.Repositories;

public class UserRepository : Repository<User, int>
{
    private readonly ICryptService _cryptService;
    public UserRepository(NetMonitorContext db, ICryptService cryptService) : base(db)
    {
        _cryptService = cryptService;
    }
}