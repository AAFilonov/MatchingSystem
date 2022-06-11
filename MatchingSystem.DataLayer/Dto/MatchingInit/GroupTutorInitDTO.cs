namespace MatchingSystem.DataLayer.Dto.MatchingInit;

public class GroupInitDto
{
    protected bool Equals(GroupInitDto other)
    {
        return name == other.name;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((GroupInitDto)obj);
    }

    public override int GetHashCode()
    {
        return (name != null ? name.GetHashCode() : 0);
    }

    public string name { get; set; }
    public bool value { get; set; } = false;
  
}