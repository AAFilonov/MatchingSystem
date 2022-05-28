using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;


namespace Service.Tutor;

public class TutorService : ITutorService
{
    private readonly ITutorRepository tutorRepository;

    public TutorService(ITutorRepository tutorRepository)
    {
        this.tutorRepository = tutorRepository;
    }

    public IterationData GetChoice(int tutorId)
    {
        var result = tutorRepository.GetChoiceByTutor(tutorId).ToList();

        var projects = result.Select(x => x.ProjectID).Distinct();

        var viewModel = new IterationData
        {
            CommonQuota = tutorRepository.GetCommonQuotaByTutor(tutorId)
        };


        foreach (var proj in projects)
        {
            viewModel.ChoiceDatas.Add(new TutorChoiceData
            {
                ProjectID = proj,
                ProjectName = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectName,
                Qty = result.FirstOrDefault(x => x.ProjectID == proj)?.Qty,
                Choices = result.Where(x => x.ProjectID == proj).OrderBy(t => t.SortOrderNumber).ToList(),
                ProjectIsClosed = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectIsClosed
            });
        }

        return viewModel;
    }

    public async void SetReady(int tutorId)
    {
            await tutorRepository.SetReadyAsync(tutorId);
    }

    public void SaveChoice(List<TutorChoice_1> data, int tutorId)
    {
            tutorRepository.SetPreferences(data, tutorId);
    }
}
