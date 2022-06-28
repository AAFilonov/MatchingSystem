using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Base;
using MatchingSystem.DataLayer.Dto.MatchingInit;
using MatchingSystem.DataLayer.IO.Params;

namespace MatchingSystem.DataLayer.Repository;

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
                        TutorId = tut.TutorId
                        ,GroupId = tutGroup.groupId
                    });   
            }
        }
    }
}