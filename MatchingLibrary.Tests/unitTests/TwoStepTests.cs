using Common.Utils;
using MatchingLibrary.Allocated;
using MatchingLibrary.Allocated.impl;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators;
using MatchingLibrary.Tests.Utils;
using NUnit.Framework;

namespace MatchingLibrary.Tests.unitTests;

public class TwoStepTests
{
    private static TwoStepAllocator allocator = new();
    private const string TestRecourceFolder = "resources/twostep/";

    
    //[Test]
    public void createJson()
    {
        var students = new List<IToOneAllocated>{
            new ComplexToOneAllocated("s1"),
            new ComplexToOneAllocated("s2"),
            new ComplexToOneAllocated("s3"),
        };
        var lecturers = new List<ITwoStepAllocated>{
            new ComplexTwoStepAllocated("L1", 2),
            new ComplexTwoStepAllocated("L2", 2),
        };
        var projects = new List<IDependentAllocated>{
            new ComplexDependedAllocated("p1", 1),
            new ComplexDependedAllocated("p2", 1),
            new ComplexDependedAllocated("p3", 1),
        };

        var data = new TwoStepAllocationJsonData();
        data.projects = projects.Cast<ComplexDependedAllocated>().ToList();
        data.lecturers = lecturers.Cast<ComplexTwoStepAllocated>().ToList();
        data.students = students.Cast<ComplexToOneAllocated>().ToList();

        data.lecturersToProjects.Add("L1", new List<string>{ "p1", "p2" });
        data.lecturersToProjects.Add("L2", new List<string>{ "p3" });

        data.lecturersPreferencesToStudents.Add("L1", new List<string>{ "s1", "s3", "s2" });
        data.lecturersPreferencesToStudents.Add("L2", new List<string>{ "s2", "s1", "s3" });

        data.studentsPreferencesToProjects.Add("s1", new List<string>{ "p1" });
        data.studentsPreferencesToProjects.Add("s2", new List<string>{ "p2" });
        data.studentsPreferencesToProjects.Add("s3", new List<string>{ "p3" });

        data.projectAssignedToStudents.Add("p1", new List<string>{ "s1" });
        data.projectAssignedToStudents.Add("p2", new List<string>{ "s2" });
        data.projectAssignedToStudents.Add("p3", new List<string>{ "s3" });

        string json = JsonUtils.ToJson(data);
        IOUtils.WriteStringToFile(json, "resources/twostep/out.json");
    }

    [TestCase("/IsFinalTests/WhenNoPreferencesSet.json", true)]
    [TestCase("/IsFinalTests/WhenPairsArePossible.json", false)]
    [TestCase("/IsFinalTests/WhenPairsAreNotPossible.json", true)]
    [Test]
    public void testIsFinal(string pathToAllocationJson, bool expectedResult)
    {
        string json = IOUtils.ReadFileAsString(TestRecourceFolder + pathToAllocationJson);
        var data = JsonUtils.FromJson<TwoStepAllocationJsonData>(json);
        ITwoStepAllocation allocation = data.ToAllocation();

        Assert.AreEqual(expectedResult, allocator.isFinal(allocation));
    }
    
    //none should be accepted
    [TestCase("ComputeIterationTests/whenNoPreferencesSet")]   
    //s1 should be rejected
    [TestCase("ComputeIterationTests/whenStudentIsNotAcceptable")]  
    //s1 should be accepted
    [TestCase("ComputeIterationTests/whenAcceptStudentAndProjectQuotaNotFull")]     
    //s2 should be rejected
    [TestCase("ComputeIterationTests/whenProjectQuotaIsFull_AndOneOverQuotaAndIsWorse")]  
    //s1 should be replaced by s2 
    [TestCase("ComputeIterationTests/whenProjectQuotaIsFull_AndOneOverQuotaAndIsBetter")] 
    //s3 should be rejected
    [TestCase("ComputeIterationTests/whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorse")] 
    //S3 is more preferable then s2 , so pair (s2 p2) will be rejected
    [TestCase("ComputeIterationTests/whenLecturerQuotaIsFull_AndOneOverQuotaAndIsBetter")] 
    //s3 should be rejected on p1 but replace s2 on p2, pair (s2 p2) will be rejected
    [TestCase("ComputeIterationTests/whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorseOnOneProjectButBetterOnOther")]
    [Test]
    public void testComputeIteration(string testFolder)
    {
        string initPath = TestRecourceFolder + testFolder + "/init.json";
        string expectedPath = TestRecourceFolder + testFolder + "/expected.json";
        string resultPath = TestRecourceFolder + testFolder + "/actual.json";
        
        //читамем данные инициализации
        string json = IOUtils.ReadFileAsString(initPath);
        var data = JsonUtils.FromJson<TwoStepAllocationJsonData>(json);
        ITwoStepAllocation allocation = data.ToAllocation();
        //выполняем итерацию
        allocator.computeIteration(allocation);
        var result = TestUtils.GetAllocationResult(allocation);
        //сохраняем результат для анализа
        var resultJson = JsonUtils.ToJson(result);
        IOUtils.WriteStringToFile(resultJson, resultPath);
        //сравниваем результат с ожидамым
        string expectedJson = IOUtils.ReadFileAsString(expectedPath);
        Assert.AreEqual(expectedJson,  resultJson);
    }
}