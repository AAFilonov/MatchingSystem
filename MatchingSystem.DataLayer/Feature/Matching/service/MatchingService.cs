using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Role.service;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.service;
using MatchingSystem.DataLayer.Model;

namespace MatchingSystem.DataLayer.Feature.Matching.service;

public class MatchingService : IMatchingService
{
    private readonly IMatchingRepository matchingRepository;
    private readonly IStageRepository stageRepository;
    private readonly IRoleRepository roleRepository;
    private readonly IMatchingRepositoryDapper matchingRepositoryDapper;

    public MatchingService(IMatchingRepository matchingRepository, IStageRepository stageRepository, IRoleRepository roleRepository, IMatchingRepositoryDapper matchingRepositoryDapper)
    {
        this.matchingRepository = matchingRepository;
        this.stageRepository = stageRepository;
        this.roleRepository = roleRepository;
        this.matchingRepositoryDapper = matchingRepositoryDapper;
    }

    public void SetStageEndDate(DateTime date, int matchingId)
    {
        var currentStage = matchingRepository.GetCurrentStage(matchingId);
        currentStage.EndDate = date;
        stageRepository.save(currentStage);
    }

    public void SetNextStage(int matchingId)
    {
        matchingRepositoryDapper.SetNextStage(matchingId);
    }
    // [napp].[goto_NextStage]  - TODO DPL-12 переписать эту хранимку на шарповой бизнеслогике
  

    public int CreateMatching(MatchingInitDto data)
    {
        Matching matching = new Matching();
        matching.MatchingName = data.name;
        matching.MatchingTypeId = (data.typeCode == "MG") ? 2 : 1;
        matching.CreatorUserId = 2;
        matching = matchingRepository.save(matching);
        return matching.MatchingId;
    }

    public int createFirstStage(int MatchingID)
    {
        Stage.StageEntity firstStage = new Stage.StageEntity();
        firstStage.StageTypeId = 1;
        firstStage.StartDate = DateTime.Now;
        firstStage.StartDate = DateTime.Now.Add(TimeSpan.FromHours(1));
        firstStage.IsCurrent = true;
        firstStage.MatchingId = MatchingID;
        firstStage = stageRepository.save(firstStage);
        return firstStage.StageId;
    }
    
    public IEnumerable<MatchingDto> GetMatchingsByUser(int userId)
    {
        IEnumerable<int?> matchingIds =roleRepository.findAll().Where(role => role.UserId.Equals(userId)).Select(role => role.MatchingId);
        return matchingRepository.find(matching => matchingIds.Contains(matching.MatchingId))
            .Select(matching => new MatchingDto(matching));
    }
}