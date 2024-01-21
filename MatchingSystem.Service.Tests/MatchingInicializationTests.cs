using System;
using System.Text;
using NUnit.Framework;
using Scrypt;

namespace MatchingSystem.Service.Tests;

[TestFixture]
public class MatchingInicializationTests
{
    //TODO тест с моками
    //IMatchingRepository  matchingRepository = new Mock<IMatchingRepository>();
    //IMatchingInitializationService  matchingInitializationService= new MatchingInitializationService();
    
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
}