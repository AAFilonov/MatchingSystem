namespace MatchingSystem.Service.StageTransition;

public interface IStageTransitionService
{
    public bool isNeedToTransit(int matchingId);
    public void MoveToNextStage(int MatchingId);
}