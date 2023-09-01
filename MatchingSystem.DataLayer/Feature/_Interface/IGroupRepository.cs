using System.Collections.Generic;
using MatchingSystem.DataLayer.Dto.MatchingInit;

namespace MatchingSystem.DataLayer.Feature._Interface
{
    public interface IGroupRepository
    {
       int CreateGroup(string groupName, int matchingId);
      void AssignGroupsToTutors(List<TutorInitDto> tutors, int matchingId);
    }
}