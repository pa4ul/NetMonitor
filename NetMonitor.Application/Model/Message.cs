namespace NetMonitor.Model;

public class Message
{
    public int Id { get; private set; }
    public virtual Host Host { get; set; }
    public virtual  Service Service { get; set; }
    public Description Description { get; set; }
    public DateTime Date { get; set; }

    public Message(Host host, Service service, Description description)
    {
        Host = host;
        Service = service;
        Description = description;
        Date = DateTime.Now;
    }
#pragma warning disable CS8618
    protected Message() { }
#pragma warning restore CS8618
}