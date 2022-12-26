using NetMonitor.Model;

namespace NetMonitor.Test;

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
        Service1.SetNormalInterval(20);

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
        Service1.SetRetryIntervall(10);

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
        Service1.SetDescription(new Description("used for testing purposes"));

        // ASSERT
        Assert.Equal("used for testing purposes",Service1.Description.description);
    }
    
    [Fact]
    public void create_warning()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        // ACT
        Service1.CreateWarning(Host1,Service1,8,new Description("CVE 123-456"));

        // ASSERT
        Assert.Equal("CVE 123-456", Service1.GetLastWarning().Description.description);
    }
    
    [Fact]
    public void review_warning()
    {
        // ARRANGE
        var Host1 = new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory"));
        var Service1 = new Service(Host1, 10, 5, new Description("PingCheck"));

        // ACT
        Service1.ReviewWarning(new Warning(Host1,Service1,8,new Description("CVE 123-456")),true,"fixed with update");
        
    
        // ASSERT
        Assert.Equal("CVE 123-456",Service1.GetLastReviwedWarning().Description.description);
    }
}