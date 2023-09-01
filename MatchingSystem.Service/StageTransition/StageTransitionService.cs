using System.Linq;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Feature.Matching.service;

namespace MatchingSystem.Service.Follow;

public class StageTransitionService : IStageTransitionService
{
    private readonly IMatchingRepository matchingRepository;
    private readonly IMatchingService matchingService;
    private readonly IProjectRepository projectRepository;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;

    public StageTransitionService(IMatchingRepository matchingRepository, ITutorRepository tutorRepository,
        IStudentRepository studentRepository, IProjectRepository projectRepository, IMatchingService matchingService)
    {
        this.matchingRepository = matchingRepository;
        this.tutorRepository = tutorRepository;
        this.studentRepository = studentRepository;
        this.projectRepository = projectRepository;
        this.matchingService = matchingService;
    }

    //isNeedToTransit    
    public bool isNeedToTransit(int matchingId)
    {
        var needToTransit = false;
        var curStageDto = matchingRepository.GetCurrentStageDto(matchingId);
        switch (curStageDto.StageTypeCode)
        {
            case 2: //переход со стадии создания проектов к заполнению списков предпочтений
                needToTransit = tutorRepository.GetTutorsByMatching(matchingId)
                    .All(x => tutorRepository.GetReadyByTutor(x.TutorID));
                break;
            case 3: //переход со стадии заполнения списков предпочтений к 1 итерации
                var studPref = studentRepository.GetStudentPreferencesByMatching(matchingId)
                    .Where(x => x.OrderNumber == 1);
                needToTransit = studPref.All(x => x.ProjectID != null);
                break;
            case 4:
                needToTransit = needToTransitionInIteration(matchingId);
                break;
            case 5: //переход со стадии ручной корректировки распределения к финалу
                needToTransit = studentRepository.GetStudentAssignedToProject(matchingId).All(x => x.ProjectID != null);
                break;
        }

        return needToTransit;
    }

    public void TransitionIfExistNeed(int MatchingId)
    {
        matchingService.SetNextStage(MatchingId);
    }

    private bool needToTransitionInIteration(int MatchingId)
    {
        var tutChoice = tutorRepository.getChoicesByMatchingCurrentStage(MatchingId);
        var projsClosed = projectRepository.GetProjectsByMatching(MatchingId).Where(x => x.IsClosed == true);

        //получить проекты которые уже закрыты на данной итерации (из списка полученных проектов)
        var queryTutCh = tutChoice.Where(c => !(from p in projsClosed select p.ProjectID).Contains(c.ProjectID));

        //проверить все ли проекты были обновлены преподавателями на текущей итерации
        return queryTutCh.Select(x => x.ProjectID)
            .All(item => queryTutCh.Where(x => x.ProjectID == item && x.UpdateDate != null).Any());
    }
}