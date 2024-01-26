using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.TutorChoise.repo;

public class TutorsChoiceRepository : GenericRepository<TutorsChoiceEntity>, ITutorsChoiceRepository
{
    public void SetPreferences(List<OldEntities.TutorChoiceDto> data, int tutorId)
    {
        var ids = data.Select(dto => dto.ChoiceID).ToList();
        var tutorChoicesToUpdate = find(entity => ids.Contains(entity.ChoiceId)).ToList();
        foreach (TutorsChoiceEntity tutorsChoiceEntity in tutorChoicesToUpdate)
        {
            OldEntities.TutorChoiceDto newChoise =
                data.First(choice1 => choice1.ChoiceID == tutorsChoiceEntity.ChoiceId);
            tutorsChoiceEntity.SortOrderNumber = newChoise.SortOrderNumber;
            tutorsChoiceEntity.IsInQuota = newChoise.IsInQuota;
        }

        updateRange(tutorChoicesToUpdate);
    }
}