namespace MatchingSystem.Service.Follow;
public interface IStageTransitionService
{
    public bool isNeedToTransit(int matchingId);
    public void TransitionIfExistNeed(int MatchingId);
}
