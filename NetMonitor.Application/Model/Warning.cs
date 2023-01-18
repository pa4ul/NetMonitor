using System.ComponentModel.DataAnnotations.Schema;

namespace NetMonitor.Model;

[Table("Warning")]

public class Warning : Message
{
    private int _priority;
    public int Priority
    {
        get => _priority;
        set
        {
            if (value>=0 && value <= 10)
                _priority = value;
            else
                throw new ArgumentException("Priority not between 0 and 10");
        }
    }
    public bool Reviewed { get; set; }
    public DateTime ReviewedDate { get; set; }
    
    public Warning(Message m, int priority, bool reviewed) : base(m.Host, m.Service, m.Description)
    {
        Priority = priority;
        Reviewed = reviewed;
        ReviewedDate = DateTime.Now;

    }
#pragma warning disable CS8618
    protected Warning() { }
#pragma warning restore CS8618
    
}