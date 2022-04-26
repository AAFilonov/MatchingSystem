using MatchingLibrary;
using TestStand.Allocated;

namespace TestStand.Algorithms;

public interface IAllocationAlgorithm<T, U>
    where T : class,IAllocated
    where U : class,IAllocated
{
    
    /// <summary>
    /// Вычисляет один шаг алгоритма, модифицирует значение переданного аргумента
    /// </summary>
    public void computeStep(OneToOneAllocation<T,U> allocation);
    public Boolean isFinal(OneToOneAllocation<T,U> allocation);
}