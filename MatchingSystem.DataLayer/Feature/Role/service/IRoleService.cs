namespace MatchingSystem.DataLayer.Feature.Role.service;

public interface IRoleService
{
    void AssignRoleForUser(int userId, int matchingId);
    
}