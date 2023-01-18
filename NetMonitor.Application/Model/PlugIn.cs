using NetMonitor.Model;

namespace NetMonitor;

public class PlugIn : Service
{
    public string Name { get; set; }
    public string URL { get; set; }
#pragma warning disable CS8618
    protected PlugIn() { }
#pragma warning restore CS8618

    public PlugIn(Service s, string name, string url) : base(s.Host, s.NormalInterval, s.RetryInterval, s.Description)
    {
        Name = name;
        URL = url;
    }
}