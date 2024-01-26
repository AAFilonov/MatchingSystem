using System.Linq;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Project.Repo;
using MatchingSystem.DataLayer.Feature.Student.repo;
using MatchingSystem.DataLayer.Feature.StudentPreference.repo;
using MatchingSystem.DataLayer.Feature.Tutor.repo;
using MatchingSystem.Service.Matching;


namespace MatchingSystem.Service.StageTransition;

[AllArgsConstructor]
public partial  class StageTransitionService : IStageTransitionService
{
    private readonly IMatchingRepository matchingRepository;
    private readonly IMatchingService matchingService;
    private readonly IStudentPreferenceRepository studentPreferenceRepository;
    private readonly IProjectRepositoryOld projectRepositoryOld;
    private readonly IStudentRepositoryOld studentRepositoryOld;
    private readonly IStudentRepository studentRepository;
    private readonly ITutorRepository tutorRepository;

    //isNeedToTransit    
    public bool isNeedToTransit(int matchingId)
    {
        var needToTransit = false;
        var curStageDto = matchingRepository.GetCurrentStageDto(matchingId);
        switch (curStageDto.StageTypeCode)
        {
            case 2: //переход со стадии создания проектов к заполнению списков предпочтений
                needToTransit = tutorRepository.find(entity => entity.MatchingId==matchingId)
                    .All(x => tutorRepository.findById(x.TutorId).IsReadyToStart ?? true);
                break;
            case 3: //переход со стадии заполнения списков предпочтений к 1 итерации
                var students = studentRepository.find(entity => entity.MatchingId == matchingId).ToList();
                needToTransit = students.All(entity => entity.StudentsPreferences.Count > 0);
                break;
            case 4:
                needToTransit = IsMoveToNextStageAvailible(matchingId);
                break;
            case 5: //переход со стадии ручной корректировки распределения к финалу
                needToTransit = studentRepositoryOld.GetStudentAssignedToProject(matchingId).All(x => x.ProjectID != null);
                break;
        }

        return needToTransit;
    }

    public void MoveToNextStage(int MatchingId)
    {
        //TODO DPL-13 проверить что смена шагов работает правильно
        //matchingService.SetNextStage(MatchingId);
    }

    private bool IsMoveToNextStageAvailible(int MatchingId)
    {
        var tutChoice = tutorRepository.getChoicesByMatchingCurrentStage(MatchingId);
        var projsClosed = projectRepositoryOld.GetProjectsByMatching(MatchingId).Where(x => x.IsClosed == true);

        //получить проекты которые уже закрыты на данной итерации (из списка полученных проектов)
        var queryTutCh = tutChoice.Where(c => !(from p in projsClosed select p.ProjectId).Contains(c.ProjectID));

        //проверить все ли проекты были обновлены преподавателями на текущей итерации
        return queryTutCh.Select(x => x.ProjectID)
            .All(item => queryTutCh.Where(x => x.ProjectID == item && x.UpdateDate != null).Any());
    }
}