using System.ComponentModel.DataAnnotations.Schema;

namespace NetMonitor.Model;

[Table("Service")]

public class Service
{
    public int Id { get; private set; }
    public virtual Host Host { get; set; }

    private int _normalInterval;

    public int NormalInterval
    {
        get => _normalInterval;
        set
        {
            if (value > 0) _normalInterval = value;
        }
    }

    private int _retryInterval;
    public int RetryInterval
    {
        get => _retryInterval;
        set
        {
            if (value > 0) _retryInterval = value;
        } 
    }
    public Description Description { get; set; }
    protected List<Message> _messages = new List<Message>();
    public virtual IReadOnlyCollection<Message> Messages => _messages;
    
    //Discriminator
    public string ServiceType { get; private set; } = default!;
    public Service(Host host, int ninterval, int rinterval, Description description)
    {
        Host = host;
        NormalInterval = ninterval;
        RetryInterval = rinterval;
        Description = description;
    }
#pragma warning disable CS8618
    protected Service() { }
#pragma warning restore CS8618

    public void AddMessage(Message m)
    {
        _messages.Add(m);
    }

    public void RemoveMessage(Message m)
    {
        _messages.Remove(m);
    }

    public Message LastProducedMessage()
    {
        return _messages[^1];
    }
}