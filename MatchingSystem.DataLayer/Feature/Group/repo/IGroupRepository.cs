using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.MatchingInit;

namespace MatchingSystem.DataLayer.Feature.Group.repo
{
    public interface IGroupRepository : IRepository<GroupEntity> 
    {
      void AssignGroupsToTutors(List<TutorInitDto> tutors, DiplomaMatchingContext? extContext = null);
      List<GroupEntity> CreateGroups(MatchingInitData data, DiplomaMatchingContext? extContext = null);
      IEnumerable<GroupDto> GetGroupsByTutor(int tutorId);
      
    }
}