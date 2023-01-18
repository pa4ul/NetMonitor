using NetMonitor.Model;

namespace NetMonitor.Test;

[Collection("Sequential")]
public class HostTests : DatabaseTest
{
    public HostTests()
    {
        _db.Database.EnsureCreated();
        var host = new Host("PC inside data center", "192.168.10.10",
            new Description("Medion PC with INTEL Core i7-13700K"));
        _db.Hosts.Add(host);
        var service = new Service(host, 100, 20,
            new Description("Current CPU-temperature", "Current CPU-temperature in Celsius"));
        host.AddService(service);
        _db.SaveChanges();
    }

    [Fact]
    public void AddServiceSuccessTest()
    {
        Assert.True(_db.Hosts.ToList().Count == 1);
    }

    [Fact]
    public void RemoveServiceSuccessTest()
    {
        var host = _db.Hosts.First();
        var service = _db.Services.First();
        host.RemoveService(service);
        _db.SaveChanges();

        _db.ChangeTracker.Clear();

        host = _db.Hosts.First();
        //Assert.Equal(0, host.ServicesInUse.Count);
        Assert.True(host.ServicesInUse.Count == 0);
    }

    [Fact]
    public void CurrentServiceQuantitySuccessTest()
    {
        Assert.True(_db.Hosts.First().CurrentServiceQuantity() == 1);
    }

    [Fact]
    public void CurrentSetupSuccessTest()
    {
        Assert.Equal(
            "Hostname: pc inside data center - IPAddress: 192.168.10.10 - Services in use: 1",
            _db.Hosts.First().CurrentSetup());
    }
}