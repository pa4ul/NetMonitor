using Microsoft.EntityFrameworkCore;
using NetMonitor.Model;

namespace NetMonitor.Infrastructure.Repositories;

public abstract class Repository<Tentity, Tkey> where Tentity : class, IEntity<Tkey> where Tkey : struct
{
    protected readonly NetMonitorContext _db;
    public IQueryable<Tentity> Set => _db.Set<Tentity>();
    public Repository(NetMonitorContext db)
    {
        _db = db;
    }

    public Tentity? FindById(Tkey id) => _db.Set<Tentity>().FirstOrDefault(e => e.Id.Equals(id));
    public Tentity? FindByGuid(Guid guid) => _db.Set<Tentity>().FirstOrDefault(e => e.Guid == guid);
    public virtual (bool success, string message) Insert(Tentity entity)
    {
        _db.Entry(entity).State = EntityState.Added;
        try
        {
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return (false, ex.InnerException?.Message ?? ex.Message);
        }
        return (true, string.Empty);
    }
    public virtual (bool success, string message) Update(Tentity entity)
    {
        if (!HasPrimaryKey(entity)) { return (false, "Missing primary key."); }

        _db.Entry(entity).State = EntityState.Modified;
        try
        {
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return (false, ex.InnerException?.Message ?? ex.Message);
        }
        return (true, string.Empty);
    }
    public virtual (bool success, string message) Delete(Tentity entity)
    {
        if (!HasPrimaryKey(entity)) { return (false, "Missing primary key."); }

        _db.Entry(entity).State = EntityState.Deleted;
        try
        {
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            return (false, ex.Message);
        }
        return (true, string.Empty);
    }

    private bool HasPrimaryKey(Tentity entity) => !entity.Id.Equals(default);
}