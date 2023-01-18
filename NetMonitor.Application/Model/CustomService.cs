namespace NetMonitor.Model;

public class CustomService : Service
{
    public string Command { get; set; }
    
#pragma warning disable CS8618
    protected CustomService() { }
#pragma warning restore CS8618
    public CustomService(Service s, string command) : base(s.Host, s.NormalInterval, s.RetryInterval, s.Description)
    {
        Command = command; 
    }
    
}