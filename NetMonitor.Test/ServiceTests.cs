using NetMonitor.Model;

namespace NetMonitor.Test;

[Collection("Sequential")]
public class ServiceTests
{
    [Fact]
    public void create_service()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        // ACT
        

        // ASSERT
        Assert.Equal(10,Service1.NormalInterval);
    }
    
    [Fact]
    public void set_normalinterval()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        // ACT
        Service1.NormalInterval=20;

        // ASSERT
        Assert.Equal(20,Service1.NormalInterval);
    }
    
    [Fact]
    public void set_retryinterval()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        // ACT
        Service1.RetryInterval=10;

        // ASSERT
        Assert.Equal(10,Service1.RetryInterval);
    }
    
    [Fact]
    public void set_description()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        // ACT
        Service1.Description=(new Description("used for testing purposes"));

        // ASSERT
        Assert.Equal("used for testing purposes",Service1.Description.description);
    }
    
    [Fact]
    public void create_custom_service()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        var CustomService = new CustomService(Service1,
            "echo $(cut -d ' ' -f 1 </proc/uptime),$(w -h | wc -l),$(cut -d ' ' -f 1-3 </proc/loadavg)");

        // ACT
        
    
        // ASSERT
        Assert.Equal("echo $(cut -d ' ' -f 1 </proc/uptime),$(w -h | wc -l),$(cut -d ' ' -f 1-3 </proc/loadavg)",CustomService.Command);
    }

    [Fact]
    public void create_plugin()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        var PlugIn = new PlugIn(Service1,"AutoLoader", "https://monitor-plugins.com/fetcher/autoloader.bin");
        

        // ACT
        
    
        // ASSERT
        Assert.Equal("https://monitor-plugins.com/fetcher/autoloader.bin",PlugIn.URL);
    }
    
  
}