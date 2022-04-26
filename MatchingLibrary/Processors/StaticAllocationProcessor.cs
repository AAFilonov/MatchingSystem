using TestStand.Algorithms;
using TestStand.Allocated;

namespace MatchingLibrary.Processors;

public class StaticAllocationProcessor<T,U>
    where T : class,ISolitaryAllocated
    where U : class,ISolitaryAllocated
{
    private IAllocationAlgorithm<T, U> algorithm;

   protected  StaticAllocationProcessor(
        IAllocationAlgorithm<T, U> algorithm)
    {
        this.algorithm = algorithm;
    }

    public OneToOneAllocation<T,U> allocate(OneToOneAllocation<T,U> allocation)
    {
        while (!algorithm.isFinal(allocation))
        {
            algorithm.computeStep(allocation);
        }

       
        return allocation;
    }
}