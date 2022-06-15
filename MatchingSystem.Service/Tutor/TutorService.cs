using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.Service.Tutor;

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

    public List<TutorInitDto> GetAllTutors()
    {
        var tutors = tutorRepository.GetAllTutors();
        var tutorDtos = new List<TutorInitDto>();
        foreach (var tutor in tutors)
        {
            //tutorDtos.Add(tutor); //(TutorDto.construct(tutor,groups));  
            tutorDtos.Add(this.construct(tutor));
        }

        return tutorDtos;
    }
    private TutorInitDto construct(DataLayer.Entities.Tutor tutor)
    {
        TutorInitDto initDto = new TutorInitDto();
        initDto.UserId = tutor.TutorID;
        initDto.nameAbbreviation = tutor.NameAbbreviation;
        initDto.isIncluded = true;
        initDto.groups = null;
        initDto.quota = 3;
        return initDto;
    }
}
