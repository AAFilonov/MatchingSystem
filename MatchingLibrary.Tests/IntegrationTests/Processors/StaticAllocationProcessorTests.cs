using MatchingLibrary.Algorithms.impl;
using MatchingLibrary.Processors.impl;
using MatchingLibrary.Tests.Utils;
using NSubstitute;
using NUnit.Framework;
using TestStand.Algorithms;
using TestStand.Allocated;

namespace MatchingLibrary.Tests;

[TestFixture]
public class StaticAllocationProcessorTests
{
    private StaticSimpleAllocationProcessor processor;
    [SetUp]
    public void Setup()
    {
        
        var alg = new DAAAlgorithm<SimpleAllocated, SimpleAllocated>();
        processor = new StaticSimpleAllocationProcessor(alg);
    }
    

    [Test]
    public void testCompute_WhenEveryoneGetFirstDesired() //каждый получит первую желаемую
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };


        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men,women);
        allocation.setPreferencesActive(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] }); //A B C
        allocation.setPreferencesActive(men[1], new List<SimpleAllocated>() { women[1], women[2], women[0] }); //B C A
        allocation.setPreferencesActive(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] }); //C A B
        
        allocation.setPreferencesPassive( women[0], new List<SimpleAllocated>() { men[0] });  //A
        allocation.setPreferencesPassive( women[1], new List<SimpleAllocated>() { men[1] });  //B
        allocation.setPreferencesPassive( women[2], new List<SimpleAllocated>() { men[2] });  //C
        
        
        Console.WriteLine(PrintUtils.toString(allocation.pairs));
        var result = processor.allocate(allocation);
        Console.WriteLine(PrintUtils.toString(result.pairs));
        //should be AA BB CC
    }
    
    [Test]
    public void testCompute_WhenSomeStaySingle() //B будет отшит как не входящий в предпочтений А и как менее желанный для С
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };


        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men,women);
        allocation.setPreferencesActive(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] }); //A B C
        allocation.setPreferencesActive(men[1], new List<SimpleAllocated>() { women[0], women[2] });           //A C 
        allocation.setPreferencesActive(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] }); //C A B
        
        allocation.setPreferencesPassive( women[0], new List<SimpleAllocated>() { men[0] });             //A
        allocation.setPreferencesPassive( women[1], new List<SimpleAllocated>() { men[0] });             //A
        allocation.setPreferencesPassive( women[2], new List<SimpleAllocated>() { men[2] , men[1] });    //C B
        
        
        Console.WriteLine(PrintUtils.toString(allocation.pairs));
        var result = processor.allocate(allocation);
        Console.WriteLine(PrintUtils.toString(result.pairs));
        Console.WriteLine(PrintUtils.toString(result.calcFinalAllocation()));
        //should be AA CC B_ _B 
    }
}