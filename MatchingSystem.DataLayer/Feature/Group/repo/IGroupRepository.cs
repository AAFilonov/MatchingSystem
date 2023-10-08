using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;

namespace MatchingSystem.DataLayer.Feature.Group.repo
{
    public interface IGroupRepository : IRepository<GroupEntity> 
    {
      void AssignGroupsToTutors(List<TutorInitDto> tutors);
      public  IEnumerable<GroupDto> getGroupsByMatching(int matchingId);
      IEnumerable<GroupDto> GetGroupsByTutor(int tutorId);
    }
}