using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Role.repo;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.DataLayer.OldDto.MatchingInit;

namespace MatchingSystem.Service.Matching;

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

    public void MoveToNextStage(int matchingId)
    {
        matchingRepositoryDapper.SetNextStage(matchingId);
    }
    // [napp].[goto_NextStage]  - TODO DPL-12 переписать эту хранимку на шарповой бизнеслогике
  

    public int CreateMatching(MatchingInitDto data)
    {
        MatchingEntity matching = new MatchingEntity();
        matching.MatchingName = data.name;
        matching.MatchingTypeId = (data.typeCode == "MG") ? 2 : 1;
        matching.CreatorUserId = 2;
        matching = matchingRepository.save(matching);
        return matching.MatchingId;
    }

    public int createFirstStage(int MatchingID)
    {
        DataLayer.Feature.Stage.StageEntity firstStage = new DataLayer.Feature.Stage.StageEntity
        {
            StageTypeId = 1,
            StartDate = DateTime.Now.Add(TimeSpan.FromHours(1)),
            IsCurrent = true,
            MatchingId = MatchingID
        };
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