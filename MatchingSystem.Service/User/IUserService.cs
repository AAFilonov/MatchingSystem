using System.Collections.Generic;
using MatchingSystem.DataLayer.OldEntities;

namespace MatchingSystem.Service.User;
public interface IUserService
{
    public IEnumerable<Matching> GetMatchingsForUser(int userId);

    public IEnumerable<Role> GetRolesForUser(int matchingId, int userId);

    public IEnumerable<DataLayer.OldEntities.User> getStudentUsersByMatching(int matchingId);
}
