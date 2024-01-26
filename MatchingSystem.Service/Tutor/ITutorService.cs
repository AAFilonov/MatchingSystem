using System.Collections.Generic;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Tutor;

public interface ITutorService
{
    public IterationData GetChoice(int tutorId);

    public void SetReady(int tutorId);

    public void SaveChoice(List<TutorChoiceDto> data, int tutorId);
}