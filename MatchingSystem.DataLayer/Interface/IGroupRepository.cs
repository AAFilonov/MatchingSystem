using System.Collections.Generic;
using System.Threading.Tasks;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Interface
{
    public interface IGroupRepository
    {
       Task<int> SetNewGroup(string groupName, int matchingId);
       Task<IEnumerable<Group>> getGroupsByMatching(int matchingId);
      void SetNew_Tutors_Groups(List<TutorInitDto> tutors, int matchingId);
    }
}