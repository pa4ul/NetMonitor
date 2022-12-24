namespace NetMonitor.Model;

public class Service
{
    public int Id { get; set; }
    public Host Host { get; set; }
    public int NormalInterval { get; set; }
    public int RetryInterval { get; set; }
    public Description Description { get; set; }
    protected List<Warning> _producedWarnings = new List<Warning>();
    public virtual IReadOnlyCollection<Warning> ProducedWarnings => _producedWarnings;

    public Service(Host host, int ninterval, int rinterval, Description description)
    {
        Host = host;
        NormalInterval = ninterval;
        RetryInterval = rinterval;
        Description = description;
    }

    public void SetNormalInterval(int n)
    {
        if (n > 0)
            NormalInterval = n;
    }

    public void SetRetryIntervall(int r)
    {
        if (r > 0)
            RetryInterval = r;
    }

    public void SetDescription(Description desc)
    {
        Description = desc;
    }

    public void CreateWarning()
    {
        //not finished
    }

    public void ReviewWarning()
    {
        //not finished
    }

    public int CalculateAveragePriority()
    {
        //not finished
        return 0;
    }

    public Warning GetLastWarning()
    {
        return null;
        //not finished
    }
}