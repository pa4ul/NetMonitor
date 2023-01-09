using System.ComponentModel.DataAnnotations.Schema;

namespace NetMonitor.Model;

[Table("Service")]

public class Service
{
    public int Id { get; private set; }
    public virtual Host Host { get; set; }
    public int NormalInterval { get; set; }
    public int RetryInterval { get; set; }
    public Description Description { get; set; }
    protected List<Warning> _producedWarnings = new List<Warning>();
    public virtual IReadOnlyCollection<Warning> ProducedWarnings => _producedWarnings;
    
    protected List<ReviewedWarning> _reviewedWarnings = new List<ReviewedWarning>();
    public virtual IReadOnlyCollection<ReviewedWarning> ReviewedWarnings => _reviewedWarnings;
    public string ServiceType { get; private set; } = default!;
    public Service(Host host, int ninterval, int rinterval, Description description)
    {
        Host = host;
        NormalInterval = ninterval;
        RetryInterval = rinterval;
        Description = description;
    }
#pragma warning disable CS8618
    protected Service() { }
#pragma warning restore CS8618
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

    public void CreateWarning(Host host, Service service, int priority, Description description)
    {
        var w = new Warning(host,service,priority,description);
        _producedWarnings.Add(w);
    }

    public void ReviewWarning(Warning w, bool f, string n)
    {
        var rw = new ReviewedWarning(w, f, n);
        _reviewedWarnings.Add(rw);
    }

    public double CalculateAveragePriority()
    {
        return _producedWarnings.Average(warning => warning.Priority);
    }

    public Warning GetLastWarning()
    {
        //same as _producedWarnings[_producedWarnings.Count - 1];
        return _producedWarnings[^1];
    }
    
    public Warning GetLastReviewedWarning()
    {
        return _reviewedWarnings[^1];
    }
}