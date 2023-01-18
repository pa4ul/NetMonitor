using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace NetMonitor.Model;

[Table("Host")]
public class Host
{
    public int Id { get; private set; }

    private string _hostname = "";

    [MaxLength(255)]
    public string Hostname
    {
        get { return _hostname; }
        set { _hostname = value!.ToLower(); }
    }

    public string? Alias { get; set; }

    private string _iPAddress = "0.0.0.0";

    public string IPAddress
    {
        get { return _iPAddress; }
        set
        {
            string pattern = @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)(\.(?!$)|$)){4}$";
            Match m = Regex.Match(value, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
                _iPAddress = value;
            else
                throw new ArgumentException("IP-Address either not version 4 or invalid");
        }
    }

    public Description Description { get; set; }
    protected List<Service> _servicesInUse = new List<Service>();
    public virtual IReadOnlyCollection<Service> ServicesInUse => _servicesInUse;

    public Host(string hostname, string ipaddress, Description description)
    {
        Hostname = hostname;
        IPAddress = ipaddress;
        Description = description;
    }
#pragma warning disable CS8618
    protected Host()
    {
    }
#pragma warning restore CS8618

    public void AddService(Service service)
    {
        _servicesInUse.Add(service);
    }

    public void RemoveService(Service service)
    {
        _servicesInUse.Remove(service);
    }

    public int CurrentServiceQuantity()
    {
        return _servicesInUse.Count;
    }

    public String CurrentSetup()
    {
        String hostname = "";
        if (Hostname.Length > 1)
        {
            hostname = $"Hostname: {Hostname} -";
        }

        String alias = "";
        if (Alias is not null && Alias.Length>1)
        {
            alias = $"Alias: {Alias} -";
        }

        return $"{hostname} {alias}IPAddress: {IPAddress} - Services in use: {CurrentServiceQuantity()}";
    }
}