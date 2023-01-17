using NetMonitor.Model;

namespace NetMonitor.Test;
[Collection("Sequential")]
public class HostTests
{
    [Fact]
    public void create_host()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        
        // ACT
        

        // ASSERT
        Assert.Equal("192.168.4.10",Host1.IPAddress);
    }
    
    [Fact]
    public void add_service()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));

        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));
        var Service2 = new Service(Host1, 20, 2, new Description("CPU_workload","Checking Workload auf x86 CPU"));
        var Service3 = new Service(Host1, 120, 100, new Description("RAM_workload","Checking Workload auf DDR4 32GB RAM"));
        
        // ACT
        Host1.AddService(Service1);
        Host1.AddService(Service2);
        Host1.AddService(Service3);

        // ASSERT
        Assert.Equal(3,Host1.CurrentServiceQuantity());
    }
    
    [Fact]
    public void remove_service()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));

        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));
        var Service2 = new Service(Host1, 20, 2, new Description("CPU_workload","Checking Workload auf x86 CPU"));
        var Service3 = new Service(Host1, 120, 100, new Description("RAM_workload","Checking Workload auf DDR4 32GB RAM"));
        
        // ACT
        Host1.AddService(Service1);
        Host1.AddService(Service2);
        Host1.AddService(Service3);
        
        Host1.RemoveService(Service1);

        // ASSERT
        Assert.Equal(2,Host1.CurrentServiceQuantity());
    }
    
   
    
    [Fact]
    public void set_ip()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        
        // ACT
        Host1.IPAddress = "192.168.0.99";

        // ASSERT
        Assert.Equal("192.168.0.99",Host1.IPAddress);
    }
    
    [Fact]
    public void set_wrong_ip()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        
        // ACT
        

        // ASSERT
        Assert.Throws<ArgumentException>(() => Host1.IPAddress = "1922.168.0.99");
    }
    
    [Fact]
    public void set_hostname()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        
        // ACT
        Host1.Hostname="Workstation 1A-B";

        // ASSERT
        Assert.Equal("workstation 1a-b",Host1.Hostname);
    }
    
    [Fact]
    public void set_description()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        
        // ACT
        Host1.Description=(new Description("PC behind Desk"));

        // ASSERT
        Assert.Equal("PC behind Desk",Host1.Description.description);
    }
}