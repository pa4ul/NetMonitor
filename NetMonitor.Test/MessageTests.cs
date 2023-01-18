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
    
    [Fact]
    public void retrieve_information_test()
    {
        // ARRANGE
        var Service1 = new Service(new Host("Workstation 1A","192.168.4.10", new Description("PC inside cisco laboratory")), 10, 5, new Description("PingCheck"));
        var Message1 = new Message(Service1.Host, Service1,
            new Description("Update finished", "Update 2.70 finished at 7:30 am UTC+3"));
        
        // ACT
        String msg1 = Message1.retrieveInformation();

        // ASSERT
        Assert.Equal("Service PingCheck on 192.168.4.10 issued a Message: Update finished",msg1);
    }
}