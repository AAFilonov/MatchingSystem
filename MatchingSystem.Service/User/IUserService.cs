using MatchingSystem.DataLayer.Interface;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Dto;

namespace Service.User;
public interface IUserService
{
    public IEnumerable<Matching> GetMatchingsForUser(int userId);

    public IEnumerable<Role> GetRolesForUser(int matchingId, int userId);
}
