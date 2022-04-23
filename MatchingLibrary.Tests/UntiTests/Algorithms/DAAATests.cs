using MatchingLibrary.Algorithms.impl;
using MatchingLibrary.Processors.impl;
using MatchingLibrary.Tests.Utils;
using NUnit.Framework;
using TestStand.Allocated;

namespace MatchingLibrary.Tests.UntiTests.Algorithms;

public class DAAATests
{
    private DAAAlgorithm<SimpleAllocated, SimpleAllocated> alg;

    [SetUp]
    public void Setup()
    {
        alg = new DAAAlgorithm<SimpleAllocated, SimpleAllocated>();
    }


    [Test]
    public void testIsFinal_WhenListsNotEmptyAndNoPairs() 
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };


        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men, women);
        allocation.setPreferencesActive(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] }); 
        allocation.setPreferencesActive(men[1], new List<SimpleAllocated>() { women[1], women[2], women[0] }); 
        allocation.setPreferencesActive(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] });

        var expectedIsFinal = false;
        var actualIsFinal = alg.isFinal(allocation);
        Assert.AreEqual(expectedIsFinal, actualIsFinal);
    }
    
    [Test]
    public void testIsFinal_WhenListsEmptyAndNoPairs() 
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };


        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men, women);
        allocation.setPreferencesActive(men[0], new List<SimpleAllocated>() { }); 
        allocation.setPreferencesActive(men[1], new List<SimpleAllocated>() { }); 
        allocation.setPreferencesActive(men[2], new List<SimpleAllocated>() { });

        var expectedIsFinal = true;
        var actualIsFinal = alg.isFinal(allocation);
        Assert.AreEqual(expectedIsFinal, actualIsFinal);
    } 
    
    [Test]
    public void testIsFinal_WhenNoListsProvided() 
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };
        
        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men, women);

        var expectedIsFinal = true;
        var actualIsFinal = alg.isFinal(allocation);
        Assert.AreEqual(expectedIsFinal, actualIsFinal);
    }
    
    [Test]
    public void testIsFinal_WhenListsAreEmptyAndThereAreSomePairs() 
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };
        
        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men, women);
        
        allocation.setPreferencesActive(men[0], new List<SimpleAllocated>() { women[0], women[1], women[2] }); 
        allocation.setPreferencesActive(men[1], new List<SimpleAllocated>() { women[1], women[2], women[0] }); 
        allocation.setPreferencesActive(men[2], new List<SimpleAllocated>() { women[2], women[0], women[1] });
        
        allocation.setPair(men[0],women[0]);
        allocation.setPair(men[1],women[1]);
        var expectedIsFinal = false;
        var actualIsFinal = alg.isFinal(allocation);
        Assert.AreEqual(expectedIsFinal, actualIsFinal);
    }
    [Test]
    public void testIsFinal_WhenListsAreNotEmptyAndThereAreSomePairs() 
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };
        
        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men, women);
        allocation.setPair(men[0],women[0]);
        allocation.setPair(men[1],women[1]);
        //Для последнего достижимых нет
        var expectedIsFinal = true;      
        var actualIsFinal = alg.isFinal(allocation);
        Assert.AreEqual(expectedIsFinal, actualIsFinal);
    }
    
        
    [Test]
    public void testIsFinal_WhenListsNotEmptyAndAllArePaired() 
    {
        List<SimpleAllocated> men = new List<SimpleAllocated>()
            { new("Alex"), new("Bob"), new("Carl") };
        List<SimpleAllocated> women = new List<SimpleAllocated>()
            { new("Alice"), new("Brie"), new("Cirno") };
        
        var allocation = new Allocation<SimpleAllocated, SimpleAllocated>(men, women);
        allocation.setPair(men[0],women[0]);
        allocation.setPair(men[1],women[1]);
        allocation.setPair(men[2],women[2]);
        var expectedIsFinal = true;
        var actualIsFinal = alg.isFinal(allocation);
        Assert.AreEqual(expectedIsFinal, actualIsFinal);
    }
}