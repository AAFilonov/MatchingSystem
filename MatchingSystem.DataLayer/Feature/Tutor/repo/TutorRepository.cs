using System.Collections.Generic;
using System.Linq;
using TutorChoiceDto = MatchingSystem.DataLayer.Feature.TutorChoice.TutorChoiceDto;

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

    public IEnumerable<TutorView> GetTutorsViews()
    {
        return _matchingSystemContext.TutorsViews.ToList();
    }
}