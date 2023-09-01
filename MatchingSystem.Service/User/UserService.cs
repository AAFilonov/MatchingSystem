using System.Collections.Generic;
using System.Linq;
using MatchingSystem.DataLayer.Feature.Matching;
using MatchingSystem.DataLayer.Feature.Matching.service;
using MatchingSystem.DataLayer.Feature.User;
using MatchingSystem.DataLayer.OldEntities;
using IUserRepository = MatchingSystem.DataLayer.Feature._Interface.IUserRepository;

namespace MatchingSystem.Service.User;

public class UserService : IUserService
{
    private readonly IMatchingRepository matchingRepository;
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository, IMatchingRepository matchingRepository)
    {
        this.matchingRepository = matchingRepository;
        this.userRepository = userRepository;
    }

    public IEnumerable<MatchingDto> GetMatchingsForUser(int userId)
    {
        var model = matchingRepository.GetMatchingsByUser(userId).OrderByDescending(matching => matching.MatchingID);
        return model;
    }

    public IEnumerable<Role> GetRolesForUser(int matchingId, int userId)
    {
        var model = userRepository.GetRolesForUserAndMatching(userId, matchingId);

        return model;
    }

    public IEnumerable<UserDto> getStudentUsersByMatching(int matchingId)
    {
        return userRepository.getStudentUsersByMatching(matchingId);
    }
}