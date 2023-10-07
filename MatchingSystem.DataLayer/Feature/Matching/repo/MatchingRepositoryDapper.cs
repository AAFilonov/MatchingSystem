using Dapper;
using MatchingSystem.DataLayer.Context;

namespace MatchingSystem.DataLayer.Feature.Matching.repo;

public class MatchingRepositoryDapper : ConnectionBase,IMatchingRepositoryDapper
{
    public MatchingRepositoryDapper(string connectionString) : base(connectionString)
    {
    }

    public void SetNextStage(int matchingId)
    {
        Connection.Query("exec napp.goto_NextStage @MatchingID",
            new {MatchingID = matchingId});
    }
}