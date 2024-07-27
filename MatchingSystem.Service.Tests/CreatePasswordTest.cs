using System;
using System.Collections.Generic;
using System.Text;
using MatchingLibrary.Allocated;
using MatchingLibrary.Allocated.impl;
using MatchingLibrary.Allocation;
using MatchingLibrary.Allocators;
using NUnit.Framework;
using Scrypt;

namespace MatchingSystem.Service.Tests;

[TestFixture]
public class CreatePasswordTest
{

    [Test]
    public void createHashes()
    {
        var pass = CreatePassword(6);
        var encoder = new ScryptEncoder();

        var result = encoder.Encode(pass);
        Console.WriteLine(pass + " " + result);
    }


    public string CreatePassword(int length)
    {
        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var res = new StringBuilder();
        var rnd = new Random();
        while (0 < length--) res.Append(valid[rnd.Next(valid.Length)]);
        return res.ToString();
    }

        private HrResidentAllocator alg;

     [SetUp]
    public void Setup()
    {
        alg = new HrResidentAllocator();
    }

   
        [Test]
        public void testIsFinal_WhenListsNotEmptyAndNoPairs()
        {
               List<IToOneAllocated> students = new List<IToOneAllocated>()
            {
                new ComplexToOneAllocated("a"),
                new ComplexToOneAllocated("b"),
                new ComplexToOneAllocated("c")
            };
            List<IToManyAllocated> lecturers = new List<IToManyAllocated>()
            {
                new ComplexToManyAllocated("A"),
                new ComplexToManyAllocated("B"),
                new ComplexToManyAllocated("C")
            };

            var allocation = new OneToManyAllocation(lecturers, students);

            Assert.AreEqual(true, alg.isFinal(allocation));
        }
    
}