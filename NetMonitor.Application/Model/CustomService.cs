namespace NetMonitor.Model;

public class CustomService : Service
{
    public string Command { get; set; }
    public string ServiceType { get; set; }

    public CustomService(Service s, string command) : base(s.Host, s.NormalInterval, s.RetryInterval, s.Description)
    {
        Command = command; 
    }
}