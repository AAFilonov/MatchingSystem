using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.TutorChoise;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Tutor.repo
{
    public interface ITutorRepository : IRepository<TutorEntity>
    {
        IEnumerable<TutorView> GetTutorsViews();
        IEnumerable<TutorChoiceDto> GetChoiceByTutor(int tutorId);
        IEnumerable<TutorChoiceDto> getChoicesByMatchingCurrentStage(int MatchingId);
        int GetNotificationsCountByTutor(int tutorId);
        void SetPreferences(IEnumerable<TutorChoice_1> newChoices, int tutorId,DiplomaMatchingContext? extContext = null);
    }
}
