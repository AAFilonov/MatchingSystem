using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Allocation = MatchingSystem.DataLayer.OldEntities.Allocation;

namespace MatchingSystem.DataLayer.Feature.Matching.service;

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