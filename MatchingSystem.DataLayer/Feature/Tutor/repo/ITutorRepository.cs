using System.Collections.Generic;
using TutorChoiceDto = MatchingSystem.DataLayer.Feature.TutorChoice.TutorChoiceDto;

namespace MatchingSystem.DataLayer.Feature.Tutor.repo
{
    public interface ITutorRepository : IRepository<TutorEntity>
    {
        IEnumerable<TutorView> GetTutorsViews();
        IEnumerable<TutorChoiceDto> GetChoiceByTutor(int tutorId, int stageId);
        IEnumerable<TutorChoiceDto> getChoicesByMatchingCurrentStage(int MatchingId);
    }
}
