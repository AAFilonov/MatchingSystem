using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.TutorChoise.repo;

public interface ITutorsChoiceRepository : IRepository<TutorsChoiceEntity>
{
    void SetPreferences(List<OldEntities.TutorChoiceDto> data, int tutorId);
}