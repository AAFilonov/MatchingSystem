using System.Collections.Generic;

namespace MatchingSystem.DataLayer.Feature.Dictionary.repo
{
    public interface IDictionaryRepository
    {
        IEnumerable<TechnologyDto> GetTechnologiesAll();
        IEnumerable<WorkDirectionDto> GetWorkDirectionsAll();
    }
}
