using NetMonitor.Model;

namespace NetMonitor.Test;

public class ServiceTests : DatabaseTest
{
    public ServiceTests()
    {
        _db.Database.EnsureCreated();
        var host = new Host("PC inside data center","192.168.10.10",new Description("Medion PC with INTEL Core i7-13700K"));
        var service = new Service(host,100,20,new Description("Current CPU-temperature","Current CPU-temperature in Celsius"));
        _db.Services.Add(service);
        var message = new Message(host, service, new Description("CPU-temperature at 43 degrees"));
        service.AddMessage(message);
        _db.SaveChanges();
    }

    [Fact]
    public void AddMessageSuccessTest()
    {
        Assert.True(_db.Messages.ToList().Count == 1);
    }
    
    [Fact]
    public void LastProducedMessageSuccessTest()
    {
        var service = _db.Services.First();
        var msg = service.LastProducedMessage();
        
        Assert.True(msg.Description.description == "CPU-temperature at 43 degrees");
    }
    
    [Fact]
    public void MessagesProducedSuccessTest()
    {
        var service = _db.Services.First();
        var messages = service.MessagesProduced();
        
        Assert.True(messages[^1].Description.description=="CPU-temperature at 43 degrees");
    }
    [Fact]
    public void ProduceWarningSuccessTest()
    {
        var host = _db.Hosts.First();
        var service = _db.Services.First();
        var message = new Message(host, service, new Description("CPU-temperature at 90 degrees"));
        _db.Services.First().ProduceWarning(host,service,message.Description,7,false);
        _db.SaveChanges();
        
        _db.ChangeTracker.Clear();

        var msg = _db.Warnings.First();
        Assert.True(msg.Description.description=="CPU-temperature at 90 degrees");
    }

    [Fact]
    public void CreatePlugInSuccessTest()
    {
        
        var service = new Service(_db.Hosts.First(),100,20,new Description("Current CPU-temperature","Current CPU-temperature in Celsius"));
        var plugin = new PlugIn(service, "Loadbalancing workload", "https://plugins.netmonitor.com/plugin=1700");
        _db.PlugIn.Add(plugin);
        _db.SaveChanges();
        
        _db.ChangeTracker.Clear();

        plugin = _db.PlugIn.First();
        Assert.True(plugin.URL == "https://plugins.netmonitor.com/plugin=1700");
    }
    
    [Fact]
    public void CreateCustomServiceSuccessTest()
    {
        
        var service = new Service(_db.Hosts.First(),100,20,new Description("Current CPU-temperature","Current CPU-temperature in Celsius"));
        var customservice = new CustomService(service, @"uptime | sed -E 's/^[^,]*up *//; s/, *[[:digit:]]* users.*//; s/min/minutes/; s/([[:digit:]]+):0?([[:digit:]]+)/\1 hours, \2 minutes/'");
        _db.CustomServices.Add(customservice);
        _db.SaveChanges();
        
        _db.ChangeTracker.Clear();

        customservice = _db.CustomServices.First();
        Assert.True(customservice.Command == @"uptime | sed -E 's/^[^,]*up *//; s/, *[[:digit:]]* users.*//; s/min/minutes/; s/([[:digit:]]+):0?([[:digit:]]+)/\1 hours, \2 minutes/'");
    }
    
}