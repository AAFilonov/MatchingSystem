using MatchingSystem.DataLayer.Feature.Stage;

namespace MatchingSystem.Service.Iterations;

public interface IIterationsService
{
    DiplomaAllocation getAllocation(StageEntity currentStage);
    bool isInFinalIteration(DiplomaAllocation allocation);
    void computeIteration(DiplomaAllocation allocation);
    void startIteration(DiplomaAllocation allocation);
    void finishIteration(DiplomaAllocation allocation);
    void generateTutorChoices(DiplomaAllocation allocation);
}