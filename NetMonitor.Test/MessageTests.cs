using NetMonitor.Model;

namespace NetMonitor.Test;

[Collection("Sequential")]

public class MessageTests
{
    [Fact]
    public void create_message()
    {
        // ARRANGE
        var Service1 = new Service(new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory")), 10, 5, new Description("PingCheck"));
        var Message1 = new Message(Service1.Host, Service1,
            new Description("Update finished", "Update 2.70 finished at 7:30 am UTC+3"));
        // ACT
        

        // ASSERT
        Assert.Equal("Update 2.70 finished at 7:30 am UTC+3",Message1.Description.longdescription);
    }
}