using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Tutor.repo;

public class TutorRepository : GenericRepository<TutorEntity>, ITutorRepository
{
    public IEnumerable<TutorChoiceDto> GetChoiceByTutor(int tutorId)
    {
        return _matchingSystemContext.TutorsChoices
            .Where(choice => choice.Project.TutorId == tutorId)
            .Select(entity => new TutorChoiceDto(entity))
            .ToList();
    }

    public IEnumerable<TutorChoiceDto> getChoicesByMatchingCurrentStage(int MatchingId)
    {
        return _matchingSystemContext.TutorsChoices
            .Where(choice => choice.Stage.MatchingId == MatchingId)
            .Select(entity => new TutorChoiceDto(entity))
            .ToList();
    }

    public int GetNotificationsCountByTutor(int tutorId)
    {
        return 0;
    }

    public void SetPreferences(IEnumerable<TutorChoice_1> newChoices, int tutorId,DiplomaMatchingContext? extContext = null)
    {
        CallInContext(context =>{
            var tutorChoicesToUpdate = context.TutorsChoices
                .Where(entity => newChoices.Any(val => val.ChoiceID == entity.ChoiceId))
                .ToList();
            foreach (var tutorsChoiceEntity in tutorChoicesToUpdate)
            {
                var newChoise = newChoices.First(choice1 => choice1.ChoiceID == tutorsChoiceEntity.ChoiceId);
                tutorsChoiceEntity.SortOrderNumber = newChoise.SortOrderNumber;
                tutorsChoiceEntity.IsInQuota = newChoise.IsInQuota;
            }

            context.TutorsChoices.UpdateRange(tutorChoicesToUpdate);
        }, extContext);
    }

    public IEnumerable<TutorView> GetTutorsViews()
    {
        return _matchingSystemContext.TutorsViews.ToList();
    }
}