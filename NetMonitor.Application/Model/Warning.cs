using System.ComponentModel.DataAnnotations.Schema;

namespace NetMonitor.Model;

[Table("Warning")]

public class Warning
{
    public int Id { get; private set; }
    public virtual Host Host { get; set; }
    public virtual Service Service { get; set; }
    public Description Description { get; set; }
    public int Priority { get; set; }
    public DateTime Date { get; set; }
    public string WarningType { get; private set; } = default!;
    
    public Warning(Host host, Service service, int priority, Description description)
    {
        Host = host;
        Service = service;
        SetPriority(priority);
        Description = description;
        Date = DateTime.Now;
    }
#pragma warning disable CS8618
    protected Warning() { }
#pragma warning restore CS8618
    public void SetDescription(Description desc)
    {
        Description = desc;
    }

    public void SetPriority(int priority)
    {
        if (priority>=0 && priority <= 10)
            Priority = priority;
        else
            throw new ArgumentException("Priority not between 0 and 10");
    }
    
}