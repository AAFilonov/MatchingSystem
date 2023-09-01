using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Matching.service;

public class MatchingRepository : Repository<Matching>, IMatchingRepository
{
    public MatchingRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<MatchingDto> GetMatchingsInfo()
    {
        throw new NotImplementedException();
    }
    
    //TODO в allocationsRepository
    public IEnumerable<Allocation> GetFinalAllocations()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Allocation> GetFinalAllocationByMatching(int MatchingId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<MatchingDto> GetMatchingsByUser(int userId)
    {
        var matchingIds = _matchingSystemContext.UsersRoles.Where(role => role.MatchingId.Equals(userId)).ToArray()
            .Select(role => role.MatchingId);
        return _matchingSystemContext
            .Matchings.Where(matching => matchingIds.Contains(matching.MatchingId))
            .Select(matching => new MatchingDto(matching));
    }

    public StageDto GetCurrentStageDto(int matchingId)
    {
        var stage = GetCurrentStage(matchingId);
        return StageDto.fromEntity(stage);
    }

    public Stage.Stage GetCurrentStage(int matchingId)
    {
        var stage = _matchingSystemContext.Matchings.Find(matchingId)?.Stages
            .FirstOrDefault(stage => stage.IsCurrent != null && (bool)stage.IsCurrent);
        return stage;
    }

    public IEnumerable<MatchingDto> GetMatchingsDto()
    {
        return _matchingSystemContext.Matchings.Select(matching => new MatchingDto(matching));
    }
    
    public IEnumerable<Matching> GetMatchings()
    {
        return _matchingSystemContext.Matchings.Include(matching =>matching.MatchingType );
    }
}