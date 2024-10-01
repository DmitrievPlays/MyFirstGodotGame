public class Resource
{
    public virtual int Id { get; set; }
    public virtual string Name { get; init; }
    public virtual string Description { get; init; }
    public virtual int MaxPerStack { get; init; }
    public virtual string Icon { get; init; }
    public virtual bool IsStackable { get; }
    public virtual ResourceTypes Type { get; init; }

    public Resource()
    {
        IsStackable = MaxPerStack > 1;
    }

    public static bool operator ==(Resource? x, Resource? y)
    {
        return x.Id == y.Id;
    }

    public static bool operator !=(Resource? x, Resource? y)
    {
        return !(x.Id == y.Id);
    }
}
