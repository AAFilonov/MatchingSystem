using MatchingLibrary.Algorithms.impl;
using MatchingLibrary.Tests.Utils;
using NUnit.Framework;
using TestStand.Allocated;

namespace MatchingLibrary.Tests.UntiTests.Algorithms;

[TestFixture]
public class SpTests
{
    private SpAlgorithm<
        SimpleAllocated,
        SimpleCapaciousAllocated,
        SimpleCapaciousAllocated> alg;

    [SetUp]
    public void Setup()
    {
        alg = new SpAlgorithm<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>();
    }


    [TestFixture]
    public class IsFinalTests : SpTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3")
            };
            List<SimpleCapaciousAllocated> lecturers = new List<SimpleCapaciousAllocated>()
            {
                new("l1", 1),
                new("l2", 2)
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 2),
                new("p3", 1)
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    lecturers);
            allocation.setProjects(lecturers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });
            allocation.setProjects(lecturers[1], new List<SimpleCapaciousAllocated>() { projects[2], });

            Assert.AreEqual(true, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereIsPreferences()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3")
            };
            List<SimpleCapaciousAllocated> lecturers = new List<SimpleCapaciousAllocated>()
            {
                new("l1", 1),
                new("l2", 2)
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 2),
                new("p3", 1)
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    lecturers);
            allocation.setProjects(lecturers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });
            allocation.setProjects(lecturers[1], new List<SimpleCapaciousAllocated>() { projects[2], });

            allocation.setStudentPreferences(students[0],
                new List<SimpleCapaciousAllocated>() { projects[0], projects[2] }); //s1: p1,p3


            Assert.AreEqual(false, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereAndAllPaired()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3")
            };
            List<SimpleCapaciousAllocated> lecturers = new List<SimpleCapaciousAllocated>()
            {
                new("l1", 1),
                new("l2", 2)
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 2),
                new("p3", 1)
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    lecturers);
            allocation.setProjects(lecturers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });
            allocation.setProjects(lecturers[1], new List<SimpleCapaciousAllocated>() { projects[2], });

            allocation.setStudentPreferences(students[0],
                new List<SimpleCapaciousAllocated>() { projects[0] }); //s1: p1,p3
            allocation.setStudentPreferences(students[1],
                new List<SimpleCapaciousAllocated>() { projects[1] }); //s1: p1,p3
            allocation.setStudentPreferences(students[2],
                new List<SimpleCapaciousAllocated>() { projects[2] }); //s1: p1,p3

            allocation.assign(students[0], projects[0]);
            allocation.assign(students[1], projects[1]);
            allocation.assign(students[2], projects[2]);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }
    }


    [TestFixture]
    public class ComputeIterationTests : SpTests
    {
        [Test]
        public void whenPreferencesOfStudentEmpty()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
            };
            List<SimpleCapaciousAllocated> lecturers = new List<SimpleCapaciousAllocated>()
            {
                new("l1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 2),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    lecturers);
            allocation.setProjects(lecturers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });

            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());

            Assert.AreEqual("pair: [l1 { p1 ( )p2 ( )}], ", resultString);
            //should be empty - no reachable pairs 
        }

        [Test]
        public void whenStundentIsNotAcceptable()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleCapaciousAllocated> lecturers = new List<SimpleCapaciousAllocated>()
            {
                new("l1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 2),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    lecturers);
            allocation.setProjects(lecturers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });

            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]}); //s1 : p1
            allocation.setLecturerPreferences(lecturers[0],new List<SimpleAllocated>(){students[1]});        //l1 : s2
            
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            //should be empty - no reachable pairs 
            Assert.AreEqual("pair: [l1 { p1 ( )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenProjectQuotaNotFull()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleCapaciousAllocated> lecturers = new List<SimpleCapaciousAllocated>()
            {
                new("l1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 2),
                new("p2", 1),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    lecturers);
            allocation.setProjects(lecturers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });

            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]});   //s1 : p1
            allocation.setLecturerPreferences(lecturers[0],new List<SimpleAllocated>(){students[0]});           //l1 : s1
            
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            //should be  t1{ p1:(s1) p2:()}
            Assert.AreEqual("pair: [l1 { p1 ( s1 )p2 ( )}], ", resultString);
        }

        [Test]
        public void whenProjectQuotaIsFull_AndOneOverQuotaAndIsWorse()
        {
            
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
            {
                new("t1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 1),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    teachers);
            allocation.setProjects(teachers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });  //l1 : p1 p2
            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]});            //s1 : p1
            allocation.setStudentPreferences(students[1],new List<SimpleCapaciousAllocated>(){projects[0]});            //s2 : p1
            allocation.setLecturerPreferences(teachers[0],new List<SimpleAllocated>(){students[0],students[1]});        //l1 : s1 s2
            
         
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            //s2 should be rejected
            //should be  t1{ p1:(s1) p2:()}
            Assert.AreEqual("pair: [t1 { p1 ( s1 )p2 ( )}], ", resultString);
            
        }
        
        [Test]
        public void whenProjectQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
            };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
            {
                new("t1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 1),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    teachers);
            allocation.setProjects(teachers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });  //l1 : p1 p2
            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]});            //s1 : p1
            allocation.setStudentPreferences(students[1],new List<SimpleCapaciousAllocated>(){projects[0]});            //s2 : p1
            allocation.setLecturerPreferences(teachers[0],new List<SimpleAllocated>(){students[1],students[0]});        //l1 : s2 s1
            
         
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            //s1 should be replaced by s1 
            //should be  t1{ p1:(s1) p2:()}
            Assert.AreEqual("pair: [t1 { p1 ( s2 )p2 ( )}], ", resultString);
            
        }

       [Test]
        public void whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorse()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3"),
            };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
            {
                new("t1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 2),
                new("p2", 1),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    teachers);
            allocation.setProjects(teachers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });  //l1 : p1 p2
            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]});            //s1 : p1
            allocation.setStudentPreferences(students[1],new List<SimpleCapaciousAllocated>(){projects[1]});            //s2 : p2
            allocation.setStudentPreferences(students[2],new List<SimpleCapaciousAllocated>(){projects[0]});            //s3 : p1
            allocation.setLecturerPreferences(teachers[0],new List<SimpleAllocated>(){students[0],students[1],students[2]});        //l1 : s1 s2 s3
            
         
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            //s3 should be rejected
            //should be  t1{ p1:(s1) p2:(s2)}
            Assert.AreEqual("pair: [t1 { p1 ( s1 )p2 ( s2 )}], ", resultString);
            
        }
        
        [Test]
        public void whenLecturerQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3"),
            };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
            {
                new("t1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 2),
                new("p2", 1),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    teachers);
            allocation.setProjects(teachers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });  //l1 : p1 p2
            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]});            //s1 : p1
            allocation.setStudentPreferences(students[1],new List<SimpleCapaciousAllocated>(){projects[1]});            //s2 : p2
            allocation.setStudentPreferences(students[2],new List<SimpleCapaciousAllocated>(){projects[0]});            //s3 : p1
            allocation.setLecturerPreferences(teachers[0],new List<SimpleAllocated>(){students[2],students[0],students[1]});        //l1 :s3 s1 s2 


            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            //S3 is more preferable then s2 , so pair s2 p2 will be rejected
            Assert.AreEqual("pair: [t1 { p1 ( s1 s3 )p2 ( )}], ", resultString);
        }
          
        [Test]
        public void whenLecturerQuotaIsFull_AndOneOverQuotaAndIsWorseOnOneProjectAndBetterOnOther()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
            {
                new("s1"),
                new("s2"),
                new("s3"),
            };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
            {
                new("t1", 2),
            };
            List<SimpleCapaciousAllocated> projects = new List<SimpleCapaciousAllocated>()
            {
                new("p1", 1),
                new("p2", 2),
            };

            var allocation =
                new TwoStepAllocation<SimpleAllocated, SimpleCapaciousAllocated, SimpleCapaciousAllocated>(students,
                    teachers);
            allocation.setProjects(teachers[0], new List<SimpleCapaciousAllocated>() { projects[0], projects[1], });  //l1 : p1 p2
            allocation.setStudentPreferences(students[0],new List<SimpleCapaciousAllocated>(){projects[0]});            //s1 : p1
            allocation.setStudentPreferences(students[1],new List<SimpleCapaciousAllocated>(){projects[1]});            //s2 : p2
            allocation.setStudentPreferences(students[2],new List<SimpleCapaciousAllocated>(){projects[0],projects[1]});            //s3 : p1 p2
            allocation.setLecturerPreferences(teachers[0],new List<SimpleAllocated>(){students[0],students[2],students[1]});        //l1 : s1 s3 s2 
            
         
            alg.computeIteration(allocation);
            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            
            //s3 should be rejected  on p1 but replace s2 on p2
            Assert.AreEqual("pair: [t1 { p1 ( s1 )p2 ( s3 )}], ", resultString);
            
        }
    }
  
}