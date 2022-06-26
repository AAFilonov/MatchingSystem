using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IGroupRepository
    {
       int CreateGroup(string groupName, int matchingId);
      void AssignGroupsToTutors(List<TutorInitDto> tutors, int matchingId);
    }
}