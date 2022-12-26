namespace NetMonitor.Model;

public class Message
{
    public int Id { get; set; }
    public Host Host { get; set; }
    public Service Service { get; set; }
    public Description Description { get; set; }
    public DateTime Date { get; set; }

    public Message(Host host, Service service, Description description)
    {
        Host = host;
        Service = service;
        Description = description;
        Date = DateTime.Now;
    }
}