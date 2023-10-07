using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature;
using MatchingSystem.DataLayer.Feature._Interface;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

public class GroupRepository : Repository<Group>, IGroupRepository
{

    public GroupRepository(DbContext context) : base(context)
    {
    }

    public void AssignGroupsToTutors(List<TutorInitDto> tutors)
    {
        foreach (var tutor in tutors)
        {
            _matchingSystemContext.TutorsGroups.AddRange(tutor.groups.Select(info =>
            {
                return new TutorsGroup()
                {
                    GroupId = info.groupId,
                    TutorId = tutor.TutorId,
                };
            }));
        }

        _matchingSystemContext.SaveChanges();
    }

    public IEnumerable<GroupDto> getGroupsByMatching(int matchingId)
    {
        return find(value => value.MatchingId == matchingId).Select(info =>
        {
            return new GroupDto()
            {
                GroupID = info.GroupId,
                GroupName = info.GroupName,
            };
        });
    }
}