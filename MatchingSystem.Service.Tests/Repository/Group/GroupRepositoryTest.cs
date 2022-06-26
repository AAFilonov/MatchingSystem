using System.Transactions;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Repository;
using MatchingSystem.Service.Monitoring;
using MatchingSystem.Service.Statistics;
using NSubstitute;
using NUnit.Framework;

namespace MatchingSystem.Service.Tests.Repository.Group;

[TestFixture]
public class GroupRepositoryTest

{
    private GroupRepository groupRepository;
    
    [SetUp]
    public void Setup()
    {
        groupRepository = new GroupRepository("data source=localhost\\SQLEXPRESS;initial catalog=DiplomaMatching;Integrated Security=True;MultipleActiveResultSets=True;");

    }

    [Test]
    public  void  testInsert_whenDataIsValid()
    {
        using (var transactionScope = new TransactionScope())
        {
            var result = groupRepository.CreateGroup("Тестовая группа 2", 1);
            Assert.AreNotEqual(null,result);
            transactionScope.Dispose();
        }
    }
 
}