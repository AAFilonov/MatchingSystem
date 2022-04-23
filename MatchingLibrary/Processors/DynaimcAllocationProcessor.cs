using TestStand.Algorithms;
using TestStand.Allocated;

namespace MatchingLibrary.Processors;

public class DynamicAllocationProcessor<T, U>
    where T :class,ISolitaryAllocated
    where U : class,ISolitaryAllocated
{
    private IAllocationAlgorithm<T, U> algorithm;

    protected DynamicAllocationProcessor(
        IAllocationAlgorithm<T, U> algorithm)
    {
        this.algorithm = algorithm;
    }

    public Allocation<T, U> computeAllocationStep(Allocation<T, U> allocation)
    {
        algorithm.computeStep(allocation);
        return allocation;
    }
}