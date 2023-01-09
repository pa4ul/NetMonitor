using System.ComponentModel.DataAnnotations.Schema;

namespace NetMonitor.Model;

public class ReviewedWarning : Warning
{
    public ReviewedWarning(Warning warning, bool f, string notes) : base(warning.Host, warning.Service,
        warning.Priority, warning.Description)
    {
        Fixed = f;
        Notes = notes;
    }
#pragma warning disable CS8618
    protected ReviewedWarning()
    {
    }
#pragma warning restore CS8618
    public bool Fixed { get; set; }
    public string Notes { get; set; }


}