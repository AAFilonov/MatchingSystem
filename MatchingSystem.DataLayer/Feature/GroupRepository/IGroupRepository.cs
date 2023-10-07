using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature._Interface
{
    public interface IGroupRepository : IRepository<Group> 
    {
      void AssignGroupsToTutors(List<TutorInitDto> tutors);
      public  IEnumerable<GroupDto> getGroupsByMatching(int matchingId);
    }
}