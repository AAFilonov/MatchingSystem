using System.Collections.Generic;
using Dapper;
using MatchingSystem.DataLayer.Context;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.Feature.Interface;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.DataLayer.Feature.Repository;

public class GroupRepository :ConnectionBase, IGroupRepository
{
    public GroupRepository(string connectionString) : base(connectionString)
    {
    }

    public  IEnumerable<Group> getGroupsByMatching(int matchingId)
    {
        return  Connection.Query<Group>("select GroupID, GroupName from Groups");
    }
    
    public int CreateGroup(string groupName,int matchingId)
    {
        return Connection.ExecuteScalar<int>(
            "insert into Groups (GroupName,MatchingID) OUTPUT INSERTED.GroupID Values(@GroupName,@MatchingId)",
            new
            {
                GroupName = groupName,
                MatchingId = matchingId
            });
    }

    public void  AssignGroupsToTutors(List<TutorInitDto> tutors, int matchingId)
    {
        foreach (var tut in tutors)
        {
            foreach (var tutGroup in tut.groups)
            {
                Connection.Execute(
                    "insert into Tutors_Groups (TutorID,GroupID) Values(@TutorId,@GroupId)",
                    new
                    {
                        tut.TutorId
                        ,GroupId = tutGroup.groupId
                    });   
            }
        }
    }
}