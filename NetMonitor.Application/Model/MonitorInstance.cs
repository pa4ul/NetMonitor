using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetMonitor.Model;

[Table("Monitor")]
public class MonitorInstance
{
    public int Id { get; private set; }
    public Guid Guid { get; private set; }

    [MaxLength(255)] public string Name { get; set; }
    protected List<Host> _hosts = new List<Host>();
    public virtual List<Host> Hosts => _hosts;

    public MonitorInstance(string name)
    {
        Name = name;
        Guid = Guid.NewGuid();
    }
#pragma warning disable CS8618
    protected MonitorInstance()
    {
    }
#pragma warning restore CS8618

    public void AddHost(Host host)
    {
        _hosts.Add(host);
    }

    public void RemoveHost(Host host)
    {
        _hosts.Remove(host);
    }

    public int CurrentQuantity()
    {
        return Hosts.Count;
    }
}