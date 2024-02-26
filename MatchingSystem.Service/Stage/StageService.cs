using System;
using System.Linq;
using Lombok.NET;
using MatchingSystem.Constants;
using MatchingSystem.DataLayer.Feature.Matching.repo;
using MatchingSystem.DataLayer.Feature.Stage;
using MatchingSystem.DataLayer.Feature.Stage.repo;
using Microsoft.Extensions.Logging;

namespace MatchingSystem.Service.Stage;

[AllArgsConstructor]
public partial class StageService : IStageService
{
    private readonly IStageRepository stageRepository;
    private readonly IMatchingRepository matchingRepository;
    private readonly ILogger<StageService> logger;

    public void MoveToNextStage(int matchingId)
    {
        logger.LogInformation("Начат перевод распределения с id={} на следующий этап", matchingId);
        var currentStage = matchingRepository.GetCurrentStage(matchingId);
        logger.LogInformation("Текущая стадия распредления {} имеет StageId={} и находится на этапе StageTypeId={}",
            matchingId,
            currentStage.StageId,
            currentStage.StageTypeId);

        switch ((StageTypeEnum)currentStage.StageTypeId)
        {
            case StageTypeEnum.MATCHING_INITIALIZATION:
                UpdateStages(currentStage, StageTypeEnum.MATCHING_PREPARATION);
                break;
            case StageTypeEnum.MATCHING_PREPARATION:
                UpdateStages(currentStage, StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES);
                break;
            case StageTypeEnum.ACCUMULATION_STUDENTS_PREFERENCES:
                UpdateStages(currentStage, StageTypeEnum.ITERATION, 1);
                break;
            case StageTypeEnum.ITERATION:
                MoveToNextInteration(currentStage);
                break;
            case StageTypeEnum.MANUAL_ADJUSTMENT:
                UpdateStages(currentStage, StageTypeEnum.FINAL);
                break;
            case StageTypeEnum.FINAL:
                //терминальный
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateStages(StageEntity currentStage, StageTypeEnum newStageType, short? iterationNumber = null)
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

    private void MoveToNextInteration(StageEntity currentStage)
    {
        UpdateStages(currentStage, StageTypeEnum.MANUAL_ADJUSTMENT);

        //TODO DPL-4 - написать логи
    }
}