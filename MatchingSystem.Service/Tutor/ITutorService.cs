using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.Service.Tutor;
public interface ITutorService
{
    public IterationData GetChoice(int tutorId);

    public void SetReady(int tutorId);

    public void SaveChoice(List<TutorChoice_1> data, int tutorId);
    public List<TutorDtoInit> GetAllTutors();
}
