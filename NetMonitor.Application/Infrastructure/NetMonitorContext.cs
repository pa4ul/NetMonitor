using Microsoft.EntityFrameworkCore;
using NetMonitor.Model;

namespace NetMonitor.Infrastructure;

public class NetMonitorContext : DbContext
{
    public NetMonitorContext(DbContextOptions opt) : base(opt) { }

    public DbSet<CustomService> CustomServices => Set<CustomService>();
    public DbSet<Description> Descriptions => Set<Description>();
    public DbSet<Host> Hosts => Set<Host>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<MonitorInstance> MonitorInstances => Set<MonitorInstance>();
    public DbSet<ReviewedWarning> ReviewedWarnings => Set<ReviewedWarning>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Warning> Warnings => Set<Warning>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Host>().OwnsOne(h => h.Description);
        modelBuilder.Entity<Service>().OwnsOne(s => s.Description);
        modelBuilder.Entity<Message>().OwnsOne(m => m.Description);
        modelBuilder.Entity<Warning>().OwnsOne(w => w.Description);

        modelBuilder.Entity<Warning>().HasDiscriminator(r => r.WarningType);
        modelBuilder.Entity<Service>().HasDiscriminator(c => c.ServiceType);
    
        modelBuilder.Entity<MonitorInstance>().HasAlternateKey(m => m.Guid);

    }
}