using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Model;

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

    [Fact]
    public void seed_database()
    {
        using var db = GetDatabase(true);

        MonitorInstance m1 = new MonitorInstance("HTL Spengergasse");

        Host h1 = new Host("Cisco Server 1", "192.168.0.10", new Description("Cisco Server at B.3.10"));
        
        
        db.MonitorInstances.Add(m1);
        db.Hosts.Add(h1);
        m1.AddHost(h1);

        db.SaveChanges();
    }
}