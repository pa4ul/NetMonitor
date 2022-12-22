using System.ComponentModel.DataAnnotations;

namespace NetMonitor.Application.Model;

public class Monitor
{
    public int Id { get; private set; }
    public Guid Guid { get; private set; }
    
    [MaxLength(255)] 
    public string Name { get; set; }
    protected List<Host> _hosts = new List<Host>();
    public virtual IReadOnlyCollection<Host> Hosts => _hosts;

    public Monitor(string name)
    {
        Name = name;
        Guid = Guid.NewGuid();
    }

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
        return _hosts.Count;
    }


}