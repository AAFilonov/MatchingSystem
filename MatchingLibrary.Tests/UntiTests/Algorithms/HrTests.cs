using MatchingLibrary.Algorithms.impl;
using MatchingLibrary.Processors.impl;
using MatchingLibrary.Tests.Utils;
using NUnit.Framework;
using TestStand.Allocated;

namespace MatchingLibrary.Tests.UntiTests.Algorithms;

[TestFixture]
public class HrTests
{
    private HrAlgorithm<SimpleAllocated, SimpleCapaciousAllocated> alg;

    [SetUp]
    public void Setup()
    {
        alg = new HrAlgorithm<SimpleAllocated, SimpleCapaciousAllocated>();
    }

    [TestFixture]
    public class IsFinalTests : HrTests
    {
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
            List<SimpleAllocated> residents = new List<SimpleAllocated>()
                { new("Alex"), new("Bob"), new("Carl") };
            List<SimpleCapaciousAllocated> hospitals = new List<SimpleCapaciousAllocated>()
                { new("A", 1), new("B", 2), new("C", 1) };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(residents, hospitals);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereIsPreferences()
        {
            List<SimpleAllocated> residents = new List<SimpleAllocated>()
                { new("Alex"), new("Bob"), new("Carl") };
            List<SimpleCapaciousAllocated> hospitals = new List<SimpleCapaciousAllocated>()
                { new("A", 1), new("B", 2), new("C", 1) };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(residents, hospitals);
            allocation.setPreferencesForStudent(residents[0],
                new List<SimpleCapaciousAllocated>() { hospitals[0], hospitals[1] }); //A B

            Assert.AreEqual(false, alg.isFinal(allocation));
        }

        [Test]
        public void testIsFinal_WhenListsNotEmptyAndThereAndAllPaired()
        {
            List<SimpleAllocated> stundents = new List<SimpleAllocated>()
                { new("Alex"), new("Bob"), new("Carl") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 1), new("B", 2), new("C", 1) };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(stundents, teachers);
            allocation.setPreferencesForStudent(stundents[0], new List<SimpleCapaciousAllocated>() { teachers[0] }); //A
            allocation.setPreferencesForStudent(stundents[0], new List<SimpleCapaciousAllocated>() { teachers[1] }); //B
            allocation.setPreferencesForStudent(stundents[0], new List<SimpleCapaciousAllocated>() { teachers[2] }); //C
            allocation.setPair(teachers[0], stundents[0]);
            allocation.setPair(teachers[1], stundents[1]);
            allocation.setPair(teachers[2], stundents[2]);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }
    }
    
    [TestFixture]
    public class ComputeIterationTests : HrTests
    {
        [Test]
        public void testComputeStep_whenPreferencesOfStudentEmpty()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
                { new("a") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 2), };

            //A  ни к кому не обратится - будет не распределен

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(students, teachers);
            allocation.setPreferencesForStudent(students[0], new List<SimpleCapaciousAllocated>() { }); // a: 

            allocation.setPreferencesForTeacher(teachers[0], new List<SimpleAllocated>() { }); //A:

            alg.computeStep(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { }], pair: [ { a }], ", resultString);
            //Should be A_ _a
        }
        
        [Test]
        public void testComputeStep_whenStundentIsNotAcceptable()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
                { new("a") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 2), };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(students, teachers);
            allocation.setPreferencesForStudent(students[0],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // a: 

            allocation.setPreferencesForTeacher(teachers[0], new List<SimpleAllocated>() { }); //A:

            alg.computeStep(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { }], pair: [ { a }], ", resultString);
            //Should be A_ _a
        }

        [Test]
        public void testComputeStep_whenQuotaNotFull()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
                { new("a") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 2), };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(students, teachers);
            allocation.setPreferencesForStudent(students[0],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); //a: A 

            allocation.setPreferencesForTeacher(teachers[0], new List<SimpleAllocated>() { students[0] }); //A: a

            alg.computeStep(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a }], ", resultString);
            //Should be Aa
        }

        [Test]
        public void testComputeStep_whenQuotaIsFull()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
                { new("a"), new("b") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 2), };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(students, teachers);
            allocation.setPreferencesForStudent(students[0],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // a: A
            allocation.setPreferencesForStudent(students[1],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // b: A

            allocation.setPreferencesForTeacher(teachers[0],
                new List<SimpleAllocated>() { students[0], students[1] }); //A: a b

            alg.computeStep(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a b }], ", resultString);
            //Should be Aab
        }

        [Test]
        public void testComputeStep_whenQuotaIsFull_AndOneOverQuotaAndIsWorse()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 2), };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(students, teachers);
            allocation.setPreferencesForStudent(students[0],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // a: A
            allocation.setPreferencesForStudent(students[1],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // b: A
            allocation.setPreferencesForStudent(students[2],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // c: A

            allocation.setPreferencesForTeacher(teachers[0],
                new List<SimpleAllocated>() { students[0], students[1], students[2] }); //A: a b c

            alg.computeStep(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a b }], pair: [ { c }], ", resultString);
            //Should be Aab _c
        }

        [Test]
        public void testComputeStep_whenQuotaIsFull_AndOneOverQuotaAndIsBetter()
        {
            List<SimpleAllocated> students = new List<SimpleAllocated>()
                { new("a"), new("b"), new("c") };
            List<SimpleCapaciousAllocated> teachers = new List<SimpleCapaciousAllocated>()
                { new("A", 2), };

            var allocation = new OneToManyAllocation<SimpleAllocated, SimpleCapaciousAllocated>(students, teachers);
            allocation.setPreferencesForStudent(students[0],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // a: A
            allocation.setPreferencesForStudent(students[1],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // b: A
            allocation.setPreferencesForStudent(students[2],
                new List<SimpleCapaciousAllocated>() { teachers[0] }); // c: A

            allocation.setPreferencesForTeacher(teachers[0],
                new List<SimpleAllocated>() { students[2], students[0], students[1] }); //A: c a b

            alg.computeStep(allocation);

            var resultString = PrintUtils.toString(allocation.calcFinalAllocation());
            Console.WriteLine(resultString);
            Assert.AreEqual("pair: [A { a c }], pair: [ { b }], ", resultString);
            //Should be Acb _b
        }
    }
}