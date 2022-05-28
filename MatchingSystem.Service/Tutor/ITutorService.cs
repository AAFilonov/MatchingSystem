using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;

namespace Service.Tutor;
public interface ITutorService
{
    public IterationData GetChoice(int tutorId);

    public void SetReady(int tutorId);

    public void SaveChoice(List<TutorChoice_1> data, int tutorId);
}
