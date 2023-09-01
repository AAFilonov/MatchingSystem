using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Interface
{
    public interface IDictionaryRepository
    {
        Task<IEnumerable<Technology>> GetTechnologiesAllAsync();
        IEnumerable<Technology> GetTechnologiesAll();
        Task<IEnumerable<WorkDirection>> GetWorkDirectionsAllAsync();
        IEnumerable<WorkDirection> GetWorkDirectionsAll();
    }
}
