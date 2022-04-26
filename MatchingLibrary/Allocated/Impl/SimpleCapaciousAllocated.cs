namespace TestStand.Allocated;

public class SimpleCapaciousAllocated : ICapaciousAllocated   ,IEquatable<SimpleCapaciousAllocated>
{
    
    public SimpleCapaciousAllocated(string name, int capacity)
    {
        this.name = name;
        this.capacity = capacity;
    }

    private string name { get; set; }
    private int  capacity { get; set; }
 
    public int getCapacity()
    {
        return capacity;
    }

    public List<IAllocated> getAssigned()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"{name}";
    }

    public IEnumerable<IAllocated> GetPreferences()
    {
        throw new NotImplementedException();
    }
    
    public bool Equals(SimpleCapaciousAllocated? other)
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
        return Equals((SimpleCapaciousAllocated)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name);
    }

   
}