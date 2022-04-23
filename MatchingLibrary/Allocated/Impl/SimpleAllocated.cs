namespace TestStand.Allocated;

public class SimpleAllocated : ISolitaryAllocated    ,IEquatable<SimpleAllocated>
{
    
    public SimpleAllocated(string name)
    {
        this.name = name;
    }

    private string name { get; set; }
    public IAllocated? pair { get; set; }
    public List<IAllocated> preferences { get; set; } = new List<IAllocated>();

    public IEnumerable<IAllocated> GetPreferences()
    {
        return preferences;
    }

    public bool isFree()
    {
        return pair == null;
    }

    public IAllocated? getPair()
    {
        return pair;
    }

    public override string ToString()
    {
        return $"{name}";
    }
    
    
    public bool Equals(SimpleAllocated? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return name == other.name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SimpleAllocated)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name);
    }
}