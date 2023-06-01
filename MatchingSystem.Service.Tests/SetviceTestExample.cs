using NUnit.Framework;

namespace MatchingSystem.Service.Tests;

[TestFixture]
public class ServiceTestExample
{
    [Test]
    public void testInsert_whenDataIsValid()
    {
        var expected = "everything is ok";
        var actual = "everything" + " is" + " ok";

        Assert.AreEqual(expected, actual);
    }
}