using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Entities;
using MatchingSystem.DataLayer.Interface;

namespace MatchingSystem.Service.User;
public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IMatchingRepository matchingRepository;

    public UserService(IUserRepository userRepository, IMatchingRepository matchingRepository)
    {
        this.matchingRepository = matchingRepository;
        this.userRepository = userRepository;
    }

    public IEnumerable<Matching> GetMatchingsForUser(int userId)
    {
        var model = matchingRepository.GetMatchingsByUser(userId).OrderByDescending(matching => matching.MatchingID);
        return model;
    }

    public IEnumerable<Role> GetRolesForUser(int matchingId, int userId)
    {
        var model = userRepository.GetRolesForUserAndMatching(userId, matchingId);

        return model;
    }

    public IEnumerable<DataLayer.Entities.User> getStudentUsersByMatching(int matchingId)
    {
        return userRepository.getStudentUsersByMatching(matchingId);
    }
}
