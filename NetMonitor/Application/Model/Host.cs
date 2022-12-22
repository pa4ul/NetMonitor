namespace NetMonitor.Application.Model;

public class Host
{
    public int Id { get; set; }
    public string Hostname { get; set; }
    public string? Alias { get; set; }
    public string IPAdress { get; set; }
    public Description Description { get; set; }
    protected List<Service> _servicesInUse = new List<Service>();
    public virtual IReadOnlyCollection<Service> ServicesInUse => _servicesInUse;
    
}