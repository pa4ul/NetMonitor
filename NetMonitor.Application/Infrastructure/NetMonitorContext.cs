using Microsoft.EntityFrameworkCore;
using NetMonitor.Model;

namespace NetMonitor.Infrastructure;

public class NetMonitorContext : DbContext
{
    public NetMonitorContext(DbContextOptions opt) : base(opt) { }

    public DbSet<CustomService> CustomServices => Set<CustomService>();
    public DbSet<Description> Descriptions => Set<Description>();
    public DbSet<Host> Hosts => Set<Host>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MonitorInstance> MonitorInstances => Set<MonitorInstance>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Warning> Warnings => Set<Warning>();
    public DbSet<PlugIn> PlugIn => Set<PlugIn>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Host>().OwnsOne(h => h.Description);
        modelBuilder.Entity<Service>().OwnsOne(s => s.Description);
        modelBuilder.Entity<Message>().OwnsOne(m => m.Description);
        modelBuilder.Entity<Warning>().OwnsOne(w => w.Description);

        modelBuilder.Entity<Service>().HasDiscriminator(s => s.ServiceType);
        modelBuilder.Entity<Message>().HasDiscriminator(m => m.MessageType);

        
        modelBuilder.Entity<MonitorInstance>().HasAlternateKey(m => m.Guid);
        modelBuilder.Entity<MonitorInstance>().Property(m => m.Guid).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Host>().HasAlternateKey(h => h.Guid);
        modelBuilder.Entity<Host>().Property(h => h.Guid).ValueGeneratedOnAdd();
        
        modelBuilder.Entity<Service>().HasAlternateKey(s => s.Guid);
        modelBuilder.Entity<Service>().Property(s => s.Guid).ValueGeneratedOnAdd();

    }

    public void Seed()
    {
        var monitorInstance1 = new MonitorInstance("Kühlraum Wien");
        var monitorInstance2 = new MonitorInstance("Serverraum Graz");
        var monitorInstance3 = new MonitorInstance("Serverraum Klagenfurt");
        var monitorInstance4 = new MonitorInstance("Mainframe Wien");
        var monitorInstance5 = new MonitorInstance("Lagerhalle Tirol");

        MonitorInstances.Add(monitorInstance1);
        MonitorInstances.Add(monitorInstance2);
        MonitorInstances.Add(monitorInstance3);
        MonitorInstances.Add(monitorInstance4);
        MonitorInstances.Add(monitorInstance5);
        SaveChanges();
        
        
        var host1 = new Host("PC_01", "192.168.10.10", new Description("PC beim Eingang","PC beim Eingang neben Feuerlöscher"));
        var host2 = new Host("PC_02", "192.168.20.5", new Description("PC neben Kantine","PC neben Kantine auf schwarzem Stehtisch"));
        var host3 = new Host("PC_03", "192.55.2.10", new Description("Laptop auf dem Serverrack","15 Zoll MacBook mit grauer Hülle auf dem mittleren Serverrack"));
        var host4 = new Host("MobilePhone_01", "192.168.1.22", new Description("Laptop auf dem Serverrack","15 Zoll MacBook mit grauer Hülle auf dem mittleren Serverrack"));
        var host5 = new Host("Heat-Sensor_01", "172.11.70.214", new Description("Heat Sensor","Heat Sensor, Cisco 200102, Version-number: CA2000"));
        var host6 = new Host("IPad_1", "172.168.26.10", new Description("Ipad auf dem Tisch","IPad Pro 2022 links auf dem Schreibtisch"));
        var host7 = new Host("Laptop_1", "172.168.21.10", new Description("Laptop auf dem Serverrack","15 Zoll MacBook mit grauer Hülle auf dem mittleren Serverrack"));

        Hosts.AddRange(host1,host2,host3,host4,host5,host6,host7);
        SaveChanges();

        monitorInstance1.AddHost(host1);
        monitorInstance1.AddHost(host2);
        monitorInstance2.AddHost(host3);
        monitorInstance3.AddHost(host4);
        monitorInstance3.AddHost(host5);
        monitorInstance4.AddHost(host6);
        monitorInstance5.AddHost(host7);
        SaveChanges();

        var service1 = new Service(host1, 100, 20, new Description("CPU-Temperatur check"));
        var service2 = new Service(host1, 50, 5, new Description("Ping check","Ping check an Webserver"));
        var service3 = new Service(host2, 200, 100, new Description("Verbindung zum Schulserver check"));
        var service4 = new Service(host3, 10, 2, new Description("CPU-Temperatur check"));
        var service5 = new Service(host3, 10, 5, new Description("CPU-Auslastung check"));
        var service6 = new Service(host3, 30, 15, new Description("Lüftung check","Lüftungs Auslastung wird überprüft"));
        Services.AddRange(service1,service2,service3,service4,service5,service6);
        SaveChanges();

        host1.AddService(service1);
        host1.AddService(service2);
        host2.AddService(service3);
        host3.AddService(service4);
        host3.AddService(service5);
        host3.AddService(service6);
        SaveChanges();

        var message1 = new Message(host1, service1, new Description("CPU-Temperatur bei 34 Grad Celsius"));
        var message2 = new Message(host1, service2, new Description("Ping erfolgreich um 14:25 UTC+1"));
        var message3 = new Message(host2, service3,
            new Description("Verbindung zum Schulserver erfolgreich um 14:30 UTC+1"));
        var message4 = new Message(host3, service4, new Description("CPU-Temperatur bei 55 Grad Celsius"));
        var message5 = new Message(host3, service5, new Description("CPU-Auslastung bei 33%"));
        var message6 = new Message(host3, service6,
            new Description("Lüftung funktioniert einwandfrei und ist auf Stufe 'Medium'"));
        var message17 = new Message(host1, service1, new Description("CPU-Temperatur bei 50 Grad Celsius"));
        var message8 = new Message(host1, service1, new Description("CPU-Lüftung wurde gesteigert"));

        
        Messages.AddRange(message1,message2,message3,message4,message5,message6,message17,message8);
        SaveChanges();

        service1.AddMessage(message1);
        service2.AddMessage(message2);
        service3.AddMessage(message3);
        service4.AddMessage(message4);
        service5.AddMessage(message5);
        service6.AddMessage(message6);
        SaveChanges();

        var message7 = new Message(host3, service6, new Description("Lüftung maximal ausgelastet um 12:32 UTC+1'", "Die Lüftung ist auf maximaler Stufe Ausgelastet"));
        var warning1 = new Warning(message7, 5, false);
        service6.AddMessage(warning1);
        Warnings.Add(warning1);
        SaveChanges();

        var service7 = new Service(host3, 400, 200, new Description("Uptime test formatiert", "Uptime test mit formatierter Response"));
        var plugIn = new PlugIn(service7, "Uptime Test formatted", "https://plugins.netmonitor.com/pid=1200");
        host3.AddService(plugIn);
        PlugIn.Add(plugIn);
        SaveChanges();

        var service8 = new Service(host3, 100, 100, new Description("Registry Snapshot"));
        var customService = new CustomService(service8,
            @"Get-ChildItem -Path HKCU:\SOFTWARE -recurse | Out-File HKCU_Software.reg");
        host3.AddService(customService);
        CustomServices.Add(customService);
        SaveChanges();
    }
}