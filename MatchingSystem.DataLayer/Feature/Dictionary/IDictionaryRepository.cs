using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature._Interface
{
    public interface IDictionaryRepository
    {
        IEnumerable<TechnologyDto> GetTechnologiesAll();
        IEnumerable<WorkDirection> GetWorkDirectionsAll();
    }
}
