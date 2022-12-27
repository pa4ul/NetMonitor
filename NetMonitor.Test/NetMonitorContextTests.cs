using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;

namespace NetMonitor.Test;

[Collection("Sequential")]
public class NetMonitorContextTests
{
    private NetMonitorContext GetDatabase(bool deleteDb = false)
    {
        var db = new NetMonitorContext(new DbContextOptionsBuilder()
            .UseSqlite("Data Source=NetMonitor.db")
            .UseLazyLoadingProxies()
            .Options);
        if (deleteDb)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        return db;
    }
    [Fact]
    public void CreateDatabaseSuccessTest()
    {
        using var db = GetDatabase(deleteDb: true);
    }
}