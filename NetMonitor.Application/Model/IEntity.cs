namespace NetMonitor.Model;

public interface IEntity<T> where T : struct
{
    T Id { get; }
    Guid Guid { get; }
}