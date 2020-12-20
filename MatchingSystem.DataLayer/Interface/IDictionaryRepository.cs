using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IDictionaryRepository
    {
        Task<IEnumerable<Technology>> GetTechnologiesAllAsync();
        IEnumerable<Technology> GetTechnologiesAll();
        Task<IEnumerable<WorkDirection>> GetWorkDirectionsAllAsync();
        IEnumerable<WorkDirection> GetWorkDirectionsAll();
    }
}
