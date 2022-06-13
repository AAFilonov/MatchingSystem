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
    
    public async Task<int> SetNewGroup(string groupName,int matchingId)
    {
        return await Connection.ExecuteScalarAsync<int>(
            "insert into Groups (GroupName,MatchingId) OUTPUT INSERTED.GroupId Values(@GroupName,@MatchingId)",
            new
            {
                GroupName = groupName
                ,MatchingId = matchingId
            });
    }

    public void  SetNew_Tutors_Groups(List<TutorInitDto> tutors, int matchingId)
    {
        foreach (var tut in tutors)
        {
            foreach (var tutGroup in tut.groups)
            {
                Connection.Execute(
                    "insert into Tutors_Groups (TutorId,GroupId) Values(@TutorId,@GroupId)",
                    new
                    {
                        TutorId = tut.TutorId
                        ,GroupId = tutGroup.groupId
                    });   
            }
        }
    }
}