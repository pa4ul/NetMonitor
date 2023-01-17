using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Model;

namespace NetMonitor.Test;

public class MonitorInstanceTests
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
    public void create_monitor()
    {
        // ARRANGE
        var db = GetDatabase(true);
        
        // ACT
        var Monitor = new MonitorInstance("Instance 1");
        db.MonitorInstances.Add(Monitor);
        db.SaveChanges();
        db.ChangeTracker.Clear();
        
        // ASSERT
        Assert.True(db.MonitorInstances.First().Name == "Instance 1");
    }
    
    [Fact]
    public void add_hosts()
    {
        // ARRANGE
        var db = GetDatabase(true);
        {
            var Monitor = new MonitorInstance("Instance 1");
            db.MonitorInstances.Add(Monitor);
            db.SaveChanges();
            db.ChangeTracker.Clear();
        }
        // ACT
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));

        // ACT
        
        var monitor = db.MonitorInstances.First();
        monitor.AddHost(Host1);
        db.SaveChanges();
        db.ChangeTracker.Clear();
        // ASSERT
        Assert.Equal(1, db.MonitorInstances.First().CurrentQuantity());
    }
    
    [Fact]
    public void remove_host()
    {
        // ARRANGE
        var Monitor = new MonitorInstance("Instance 1");
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Host2 = new Host("Workstation 2B","192.168.20.2", new Description("PC at office"));
        var Host3 = new Host("Workstation 3C","192.168.30.12", new Description("Laptop for product testing"));

        // ACT
        Monitor.AddHost(Host1);
        Monitor.AddHost(Host2);
        Monitor.AddHost(Host3);
        Monitor.RemoveHost(Host1);

        // ASSERT
        Assert.Equal(2, Monitor.CurrentQuantity());
    }
}