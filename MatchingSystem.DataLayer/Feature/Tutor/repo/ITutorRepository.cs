using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Tutor.repo
{
    public interface ITutorRepository : IRepository<TutorEntity>
    {
        IEnumerable<TutorView> GetTutorsViews();
        IEnumerable<TutorInitDto> CreateTutors(List<TutorInitDto> tuts, int matchingId);
        IEnumerable<TutorChoiceDto> GetChoiceByTutor(int tutorId);
        IEnumerable<TutorChoiceDto> getChoicesByMatchingCurrentStage(int MatchingId);
        int GetNotificationsCountByTutor(int tutorId);
        void SetPreferences(IEnumerable<TutorChoice_1> newChoices, int tutorId);
    }
}
