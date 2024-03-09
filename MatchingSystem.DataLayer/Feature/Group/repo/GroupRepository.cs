using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature._LinkTables;
using MatchingSystem.DataLayer.Feature.Tutor;
using MatchingSystem.DataLayer.OldDto.MatchingInit;

namespace MatchingSystem.DataLayer.Feature.Group.repo;

public class GroupRepository : GenericRepository<GroupEntity>, IGroupRepository
{
    public void AssignGroupsToTutors(List<TutorInitDto> tutors, DiplomaMatchingContext? extContext = null)
    {
        CallInContext(context =>
        {
            foreach (var tutor in tutors)
                context.TutorsGroups.AddRange(tutor.groups.Select(info =>
                {
                    return new TutorsGroup
                    {
                        GroupId = info.groupId,
                        TutorId = tutor.TutorId
                    };
                }));
        }, extContext);
    }

    public List<GroupEntity> CreateGroups(MatchingInitData data, DiplomaMatchingContext? extContext = null)
    {
        var groups = data.groupRecords.Select(item => new GroupEntity
        {
            GroupName = item.name,
            MatchingId = data.matching.matchingId
        }).ToList();
        saveRange(groups, extContext);
        return groups;
    }

    public IEnumerable<GroupDto> GetGroupsByTutor(int tutorId)
    {
        return find(value => value.TutorsGroups.Any(tutorGroup => tutorGroup.TutorId == tutorId))
            .Select(info => new GroupDto(info));
    }

    public IEnumerable<GroupDto> GetGroups()
    {
        return _matchingSystemContext.Groups.ToList().Select(group => new GroupDto(group));
    }
}