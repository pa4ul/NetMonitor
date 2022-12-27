using System.Text.RegularExpressions;

namespace NetMonitor.Model;

public class Host
{
    public int Id { get; private set; }
    public string Hostname { get; set; }
    public string? Alias { get; set; }
    public string? IPAddress { get; set; }
    public Description Description { get; set; }
    protected List<Service> _servicesInUse = new List<Service>();
    public virtual IReadOnlyCollection<Service> ServicesInUse => _servicesInUse;

    public Host(string hostname, string ipaddress, Description description)
    {
        Hostname = hostname;
        SetIP(ipaddress);
        Description = description;
    }
#pragma warning disable CS8618
    protected Host() { }
#pragma warning restore CS8618

    public void AddService(Service service)
    {
        _servicesInUse.Add(service);
    }

    public void RemoveService(Service service)
    {
        _servicesInUse.Remove(service);
    }

    public void SetAlias(string alias)
    {
        Alias = alias;
    }

    public void SetIP(string ip)
    {
        string pattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)(\.(?!$)|$)){4}$";
        Match m = Regex.Match(ip, pattern, RegexOptions.IgnoreCase);
        if (m.Success)
            IPAddress = ip;
        else
            throw new ArgumentException("IP-Address either not version 4 or invalid");
    }

    public void SetHostname(string hostname)
    {
        Hostname = hostname;
    }

    public void SetDescription(Description desc)
    {
        Description = desc;
    }

    public int CurrentServiceQuantity()
    {
        return _servicesInUse.Count;
    }
    
}