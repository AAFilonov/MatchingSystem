using TestStand.Algorithms;
using TestStand.Allocated;

namespace MatchingLibrary.Processors.impl;

public class StaticSimpleAllocationProcessor : StaticAllocationProcessor<SimpleAllocated,SimpleAllocated>
{
    public StaticSimpleAllocationProcessor(IAllocationAlgorithm<SimpleAllocated, SimpleAllocated> algorithm) : base(algorithm)
    {
    }
}