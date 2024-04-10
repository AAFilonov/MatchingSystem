using System;
using Lombok.NET;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using MatchingSystem.Service.Iterations;
using Microsoft.Extensions.Logging;

namespace MatchingSystem.Service.Stage;

[AllArgsConstructor]
public partial class StageService : IStageService
{
    private readonly IStageRepository stageRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly IIterationsService iterationsService;
    private readonly ILogger<StageService> logger;

    public void MoveToNextStage(int matchingId)
    {
        logger.LogInformation("Начат перевод распределения с id={} на следующий этап", matchingId);
        StageEntity currentStage = matchingRepository.GetCurrentStage(matchingId);
        logger.LogInformation("Текущая стадия распредления {} имеет StageId={} и находится на этапе StageTypeId={}",
            matchingId,
            currentStage.StageId,
            currentStage.StageTypeId);

        switch ((StageTypeEnum)currentStage.StageTypeId)
        {
            case StageTypeEnum.MATCHING_INITIALIZATION:
                CreateNewStage(currentStage, StageTypeEnum.MATCHING_PREPARATION);
                break;
            case StageTypeEnum.MATCHING_PREPARATION:
                CreateNewStage(currentStage, StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES);
                break;
            case StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES:
                CreateNewStage(currentStage, StageTypeEnum.ITERATION, 1);
                //TODO DPL-4 MoveToNextInteration(currentStage); // обработка связанная с созданием TutorChoise
                break;
            case StageTypeEnum.ITERATION:
                CreateNewStage(currentStage, StageTypeEnum.MANUAL_ADJUSTMENT, 1);
                //MoveToNextInteration(currentStage);
                break;
            case StageTypeEnum.MANUAL_ADJUSTMENT:
                CreateNewStage(currentStage, StageTypeEnum.FINAL);
                break;
            case StageTypeEnum.FINAL:
                //терминальный
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CreateNewStage(StageEntity currentStage, StageTypeEnum newStageType, short? iterationNumber = null)
    {
        logger.LogInformation("Выполняется переход с этапа {} на этап {}({})", currentStage.StageTypeId,
            (int)newStageType, newStageType);
        stageRepository.CallInTransaction(context =>
        {
            currentStage.IsCurrent = false;
            currentStage.EndDate = DateTime.Now;
            stageRepository.update(currentStage, context);

            var newStage = new StageEntity
            {
                StageTypeId = (int)newStageType,
                IterationNumber = iterationNumber,
                StartDate = DateTime.Now,
                IsCurrent = true,
                MatchingId = currentStage.MatchingId,
                EndPlanDate = null,
                EndDate = null,
            };

            stageRepository.save(newStage, context);
        });
        logger.LogInformation("Перевод распределения с id={} на этап {} завершен успешно",
            currentStage.MatchingId, newStageType);
    }
    
    /* TODO DPL-4
     метод вызывается в конце прошлой итерации или на шаге StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES
     генерим Allocation из данных в базе
     сначала мы прогоняем алгоритм матчинга для текущего состояния базы, в Allocation назначения InDTO
     затем мы создаем новый Stage со следующим номером итерации
     затем создаем TutorChoise для этой итерации  по в Allocation - назначения InDTO дублируем в назначения InDB
     при этом существующие назначения дублируются в новую итерацию, а новые создаются с нуля
     затем провятем является ли состояние распределения финальным
        если да мы создаем новый Stage c StageTypeEnum.MANUAL_ADJUSTMENT
    */
    private void MoveToNextIteration(StageEntity currentStage)
    {
        DiplomaAllocation allocation = iterationsService.getAllocation(currentStage);
        if (!iterationsService.isInFinalIteration(allocation))
        {
            CreateNewStage(currentStage, StageTypeEnum.ITERATION, 1);
            iterationsService.computeIteration(allocation);
        }
        else
        {
            CreateNewStage(currentStage, StageTypeEnum.MANUAL_ADJUSTMENT);
        }
    }
}