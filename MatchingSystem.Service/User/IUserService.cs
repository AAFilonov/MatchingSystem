using System.Collections.Generic;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.User;

public interface IUserService
{
    public IEnumerable<MatchingDto> GetMatchingsForUser(int userId);

    public IEnumerable<Role> GetRolesForUser(int matchingId, int userId);

    public IEnumerable<UserDto> getStudentUsersByMatching(int matchingId);
}