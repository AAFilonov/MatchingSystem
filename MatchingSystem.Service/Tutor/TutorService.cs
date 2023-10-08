using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.CommonQuota;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.Tutor;

public class TutorService : ITutorService
{
    private readonly ITutorRepository tutorRepository;
    private readonly ICommonQuotaRepository commonQuotaRepository;

    public TutorService(ITutorRepository tutorRepository, ICommonQuotaRepository commonQuotaRepository)
    {
        this.tutorRepository = tutorRepository;
        this.commonQuotaRepository = commonQuotaRepository;
    }

    public IterationData GetChoice(int tutorId)
    {
        var result = tutorRepository.GetChoiceByTutor(tutorId).ToList();

        var projects = result.Select(x => x.ProjectID).Distinct();

        var viewModel = new IterationData
        {
            CommonQuota = commonQuotaRepository.find( entity =>entity.TutorId== tutorId).First().Qty
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
       tutorRepository.save(tutor);
    }

    public void SaveChoice(List<TutorChoice_1> data, int tutorId)
    {
        tutorRepository.SetPreferences(data, tutorId);
    }
}