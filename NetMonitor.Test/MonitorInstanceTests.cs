using NetMonitor.Model;

namespace NetMonitor.Test;

public class MonitorInstanceTests
{
    [Fact]
    public void create_monitor()
    {
        // ARRANGE
        var Monitor = new MonitorInstance("Instance 1");

        // ACT
        

        // ASSERT
        Assert.Equal("Instance 1", Monitor.Name);
    }
    
    [Fact]
    public void add_hosts()
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
        // ASSERT
        Assert.Equal(3, Monitor.CurrentQuantity());
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