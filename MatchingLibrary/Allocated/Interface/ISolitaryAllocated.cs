namespace TestStand.Allocated;

public interface ISolitaryAllocated:IAllocated
{
    public Boolean isFree();
    public IAllocated? getPair();
}