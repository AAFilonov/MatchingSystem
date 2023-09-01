using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.User.service;

public class UserRepository :Repository<User>,IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    public User? getByLogin(string login)
    {
        return _matchingSystemContext.Users.Single(user => user.Login.Equals(login));
    }
}