using NetMonitor.Model;

namespace NetMonitor;

public class PlugIn : Service
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string URL { get; set; }
    public string ServiceType { get; set; }

    public PlugIn(Service s, string name, string url) : base(s.Host, s.NormalInterval, s.RetryInterval, s.Description)
    {
        Name = name;
        URL = url;
    }
}