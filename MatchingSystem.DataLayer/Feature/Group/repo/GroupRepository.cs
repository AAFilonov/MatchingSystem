using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Tutor;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Group.repo;

public class GroupRepository : GenericRepository<GroupEntity>, IGroupRepository
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
        return find(value => value.MatchingId == matchingId).Select(info => new GroupDto(info));
    }

    public IEnumerable<GroupDto> GetGroupsByTutor(int tutorId)
    {
        return find(value => value.TutorsGroups.Any(tutorGroup => tutorGroup.TutorId == tutorId))
            .Select(info => new GroupDto(info));
    }
}