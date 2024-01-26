using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.OldEntities;
using TutorChoiceDto = MatchingSystem.DataLayer.Feature.TutorChoise.TutorChoiceDto;

namespace MatchingSystem.DataLayer.Feature.Tutor.repo
{
    public interface ITutorRepository : IRepository<TutorEntity>
    {
        IEnumerable<TutorView> GetTutorsViews();
        IEnumerable<TutorChoiceDto> GetChoiceByTutor(int tutorId);
        IEnumerable<TutorChoiceDto> getChoicesByMatchingCurrentStage(int MatchingId);
    }
}
