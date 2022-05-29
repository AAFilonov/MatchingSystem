using System.Collections.Generic;
using MatchingSystem.DataLayer.Entities;

namespace MatchingSystem.Service.User;
public interface IUserService
{
    public IEnumerable<Matching> GetMatchingsForUser(int userId);

    public IEnumerable<Role> GetRolesForUser(int matchingId, int userId);
}
