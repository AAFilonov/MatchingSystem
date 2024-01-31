using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.TutorChoice.repo;

public interface ITutorsChoiceRepository : IRepository<TutorsChoiceEntity>
{
    void SetPreferences(List<OldEntities.TutorChoiceDto> data, int tutorId);
}