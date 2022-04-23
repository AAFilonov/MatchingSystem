using TestStand.Allocated;

namespace TestStand.Algorithms;

public interface IOneToOneAllocationAlgorithm<T,U>:IAllocationAlgorithm<T,U>
    where T : class,ISolitaryAllocated
    where U : class,ISolitaryAllocated
{
    
}