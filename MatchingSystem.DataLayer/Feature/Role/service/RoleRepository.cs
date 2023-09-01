using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Role.service;

public class RoleRepository:Repository<Role>,IRoleRepository
{
    public RoleRepository(DbContext context) : base(context)
    {
    }

    public IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<RoleMatching> GetRolesForUser(int userId)
    {
        throw new System.NotImplementedException();
    }
}