using System;
using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.User.service;
using MatchingSystem.DataLayer.Model;
using MatchingSystem.DataLayer.OldEntities;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.Role.service;

public class RoleRepository:Repository<UsersRole>,IRoleRepository
{
    private IUserRepositoryOld userRepositoryOld;
    public RoleRepository(DbContext context, IUserRepositoryOld userRepositoryOld) : base(context)
    {
        this.userRepositoryOld = userRepositoryOld;
    }

    public IEnumerable<RoleDto> GetRolesForUserAndMatching(int userId, int matchingId)
    {
        return findAll().Where(role => role.MatchingId == matchingId && role.UserId == userId)
            .ToList() 
            .Select(role =>
            {
                var dto = new RoleDto();
                dto.RoleCode = role.Role.RoleCode;
                dto.RoleName = role.Role.RoleName;
                dto.RoleName_ru = role.Role.RoleNameRu;
                return dto;
            });
    }

    public IEnumerable<RoleMatching> GetRolesForUser(int userId)
    {
        var UserRoles = findAll().Where(role => role.UserId == userId).ToList();
        var roles = UserRoles.Select(userRole =>
        {
            var role = new RoleMatching();
            role.UserId = userId;
            role.RoleName = userRole.Role.RoleName ?? "Ошибка! Роль не определена";
            role.RoleCode = userRole.Role.RoleCode ?? 0;
            role.RoleName_ru = userRole.Role.RoleNameRu ?? "Ошибка! Роль не определена";
            role.MatchingId = (int)userRole.MatchingId;
            role.MatchingName = userRole.Matching.MatchingName;
            role.MatchingTypeCode = userRole.Matching.MatchingType.MatchingTypeId;
            role.MatchingTypeName_ru = userRole.Matching.MatchingType.MatchingTypeNameRu;
            role.StudentID = userRole.StudentId;
            role.TutorID = userRole.TutorId;
            return role;
        });
        return roles;
    }

    public void AssignRoleForUser(int userId, int matchingId, int roleId)
    {
        userRepositoryOld.AssignRoleForUser(userId,matchingId,roleId);
    }
}