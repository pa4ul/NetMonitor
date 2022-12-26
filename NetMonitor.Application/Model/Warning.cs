namespace NetMonitor.Model;

public class Warning
{
    public int Id { get; set; }
    public Host Host { get; set; }
    public Service Service { get; set; }
    public Description Description { get; set; }
    public int Priority { get; set; }
    public DateTime Date { get; set; }
    

    public Warning(Host host, Service service, int priority, Description description)
    {
        Host = host;
        Service = service;
        Priority = priority;
        Description = description;
    }

    public void SetDescription(Description desc)
    {
        Description = desc;
    }

    public void SetPriority(int priority)
    {
        Priority = priority;
    }

}