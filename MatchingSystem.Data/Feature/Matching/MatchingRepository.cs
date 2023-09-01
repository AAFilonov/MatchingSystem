using System;
using MatchingSystem.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Matching;

public class MatchingRepository :  Repository<Data.Model.Matching>,IMatchingRepository
{
    public MatchingRepository(DbContext context) : base(context)
    {
    }
      
    public Stage GetCurrentStage(int matchingId)
    {
        throw new NotImplementedException();
    }

}