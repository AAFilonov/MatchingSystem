using System.Collections.Generic;
using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.CommonQuota.repo;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.Feature.TutorChoice.repo;
using MatchingSystem.DataLayer.OldDto;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Tutor;

[AllArgsConstructor]
public partial class TutorService : ITutorService
{
    private readonly ITutorRepository tutorRepository;
    private readonly ITutorsChoiceRepository tutorsChoiceRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IQuotaRequestRepository quotaRequestRepository;

    public IterationData GetChoice(int tutorId)
    {
        TutorEntity tutor = tutorRepository.findById(tutorId);
        StageEntity currentStage= matchingRepository.GetCurrentStage(tutor!.MatchingId);
        List<DataLayer.Feature.TutorChoice.TutorChoiceDto> result = tutorRepository.GetChoiceByTutor(tutorId,currentStage.StageId ).ToList();

        IEnumerable<int?> projects = result.Select(x => x.ProjectID).Distinct();

        var viewModel = new IterationData
        {
            CommonQuota = quotaRequestRepository.GetActiveTutorQuota(tutorId).Qty
        };


        foreach (var proj in projects)
            viewModel.ChoiceDatas.Add(new TutorChoiceData
            {
                ProjectID = proj,
                ProjectName = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectName,
                Qty = result.FirstOrDefault(x => x.ProjectID == proj)?.Qty,
                Choices = result.Where(x => x.ProjectID == proj).OrderBy(t => t.SortOrderNumber).ToList(),
                ProjectIsClosed = result.FirstOrDefault(x => x.ProjectID == proj)?.ProjectIsClosed
            });

        return viewModel;
    }

    public void SetReady(int tutorId)
    {
       TutorEntity tutor = tutorRepository.findById(tutorId);
       tutor.IsReadyToStart = true;
       tutorRepository.update(tutor);
    }

    public void SaveChoice(List<TutorChoiceDto> data, int tutorId)
    {
        tutorsChoiceRepository.SetPreferences(data, tutorId);
    }
}