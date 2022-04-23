namespace TestStand.Allocated;

public interface IAllocated
{
    public IEnumerable<IAllocated> GetPreferences();
}