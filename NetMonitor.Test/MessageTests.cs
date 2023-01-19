using NetMonitor.Model;

namespace NetMonitor.Test;

[Collection("Sequential")]
public class MessageTests : DatabaseTest
{
    public MessageTests()
    {
        _db.Database.EnsureCreated();
        _db.Database.EnsureCreated();
        var host = new Host("PC inside data center","192.168.10.10",new Description("Medion PC with INTEL Core i7-13700K"));
        var service = new Service(host,100,20,new Description("Current CPU-temperature","Current CPU-temperature in Celsius"));
        var message = new Message(host, service, new Description("CPU-temperature at 43 degrees"));
        _db.Messages.Add(message);
        _db.SaveChanges();
    }

    [Fact]
    public void RetrieveInformationSuccessTest()
    {
        var msg = _db.Messages.First();
        
        Assert.Equal("Service Current CPU-temperature on 192.168.10.10 issued a Message: CPU-temperature at 43 degrees",msg.retrieveInformation());
    }
    
    //failure tests

    [Fact]
    public void PriorityFailureTest()
    {
        var host = new Host("PC inside data center","192.168.10.10",new Description("Medion PC with INTEL Core i7-13700K"));
        var service = new Service(host,100,20,new Description("Current CPU-temperature","Current CPU-temperature in Celsius"));
        var message = new Message(host, service, new Description("CPU-temperature at 90 degrees"));
        var warning = new Warning(message, 6, false);
        _db.SaveChanges();
        _db.ChangeTracker.Clear();
        
        Assert.Throws<ArgumentException>(() => warning.Priority=17);

    }
}