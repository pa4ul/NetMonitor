using Bogus;
using NetMonitor.Model;

namespace NetMonitor.Test;

[Collection("Sequential")]
public class NetMonitorContextTests : DatabaseTest
{
    [Fact]
    public void CreateDatabaseTest()
    {
        _db.Database.EnsureCreated();
    }

    [Fact]
    public void SeedDatabase()
    {
        _db.Database.EnsureDeleted();
        _db.Database.EnsureCreated();

        var monitorInstance1 = new MonitorInstance("Kühlraum Wien");
        var monitorInstance2 = new MonitorInstance("Serverraum Graz");

        _db.MonitorInstances.Add(monitorInstance1);
        _db.MonitorInstances.Add(monitorInstance2);

        var host1 = new Host("PC_01", "192.168.10.10", new Description("PC beim Eingang","PC beim Eingang neben Feuerlöscher"));
        var host2 = new Host("PC_02", "192.168.20.5", new Description("PC neben Kantine","PC neben Kantine auf schwarzem Stehtisch"));
        var host3 = new Host("PC_03", "192.168.70.10", new Description("Laptop auf dem Serverrack","15 Zoll MacBook mit grauer Hülle auf dem mittleren Serverrack"));

        monitorInstance1.AddHost(host1);
        monitorInstance1.AddHost(host2);
        monitorInstance2.AddHost(host3);

        var service1 = new Service(host1, 100, 20, new Description("CPU-Temperatur check"));
        var service2 = new Service(host1, 50, 5, new Description("Ping check","Ping check an Webserver"));
        var service3 = new Service(host2, 200, 100, new Description("Verbindung zum Schulserver check"));
        var service4 = new Service(host3, 10, 2, new Description("CPU-Temperatur check"));
        var service5 = new Service(host3, 10, 5, new Description("CPU-Auslastung check"));
        var service6 = new Service(host3, 30, 15, new Description("Lüftung check","Lüftungs Auslastung wird überprüft"));
        var service7 = new Service(host3, 400, 200, new Description("Uptime test formatiert","Uptime test mit formatierter Response"));
        var service8 = new Service(host3, 100, 100, new Description("Registry Snapshot"));


        host1.AddService(service1);
        host1.AddService(service2);
        host2.AddService(service3);
        host3.AddService(service4);
        host3.AddService(service5);
        host3.AddService(service6);

        var message1 = new Message(host1, service1, new Description("CPU-Temperatur bei 34 Grad Celsius"));
        var message2 = new Message(host1, service2, new Description("Ping erfolgreich um 14:25 UTC+1"));
        var message3 = new Message(host2, service3,
            new Description("Verbindung zum Schulserver erfolgreich um 14:30 UTC+1"));
        var message4 = new Message(host3, service4, new Description("CPU-Temperatur bei 55 Grad Celsius"));
        var message5 = new Message(host3, service5, new Description("CPU-Auslastung bei 33%"));
        var message6 = new Message(host3, service6,
            new Description("Lüftung funktioniert einwandfrei und ist auf Stufe 'Medium'"));
        var message7 = new Message(host3, service6, new Description("Lüftung maximal ausgelastet um 12:32 UTC+1'","Die Lüftung ist auf maximaler Stufe Ausgelastet"));

        service1.AddMessage(message1);
        service2.AddMessage(message2);
        service3.AddMessage(message3);
        service4.AddMessage(message4);
        service5.AddMessage(message5);
        service6.AddMessage(message6);

        var warning1 = new Warning(message7, 5, false);
        service6.AddMessage(warning1);

        var plugIn = new PlugIn(service7, "Uptime Test formatted", "https://plugins.netmonitor.com/pid=1200");
        host3.AddService(plugIn);

        var customService = new CustomService(service8,
            @"Get-ChildItem -Path HKCU:\SOFTWARE -recurse | Out-File HKCU_Software.reg");
        host3.AddService(customService);

        _db.SaveChanges();

        //ASSERT
        _db.ChangeTracker.Clear();
        Assert.True(_db.MonitorInstances.ToList().Count > 0);
        Assert.True(_db.Hosts.ToList().Count > 0);
        Assert.True(_db.Services.ToList().Count > 0);
        Assert.True(_db.Messages.ToList().Count > 0);
    }
}