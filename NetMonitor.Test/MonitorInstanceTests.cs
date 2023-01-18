using NetMonitor.Model;

namespace NetMonitor.Test;

public class MonitorInstanceTests : DatabaseTest
{
    public MonitorInstanceTests()
    {
        _db.Database.EnsureCreated();
        var monitorinstance = new MonitorInstance("data center Donaustadt");
        _db.MonitorInstances.Add(monitorinstance);
        var host = new Host("PC inside data center","192.168.10.10",new Description("Medion PC with INTEL Core i7-13700K"));
        
        monitorinstance.AddHost(host);
        _db.SaveChanges();
    }
    
    [Fact]
    public void AddHostSuccessTest()
    {
        Assert.True(_db.MonitorInstances.First().CurrentQuantity() == 1);
    }
    
    [Fact]
    public void RemoveHostSuccessTest()
    {
        var monitorinstance = _db.MonitorInstances.First();
        var host = _db.Hosts.First();
        monitorinstance.RemoveHost(host);
        _db.SaveChanges();
        
        _db.ChangeTracker.Clear();
        
        monitorinstance = _db.MonitorInstances.First();
        Assert.Equal(0, monitorinstance.CurrentQuantity());
    }
    
    [Fact]
    public void CurrentQuantitySuccessTest()
    {
        Assert.Equal(1,_db.MonitorInstances.First().CurrentQuantity());
    }
}