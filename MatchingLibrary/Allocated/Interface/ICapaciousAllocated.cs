namespace TestStand.Allocated;

public interface ICapaciousAllocated:IAllocated
{
    public int getCapacity();
    public List<IAllocated> getAssigned();
}