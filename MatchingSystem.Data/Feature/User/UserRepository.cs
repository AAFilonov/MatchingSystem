using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.DataLayer.Feature.User;

public class UserRepository :Repository<Data.Model.User>,IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    public Data.Model.User? FindByLogin(string login)
    {
        return _matchingSystemContext.Users.Single(user => user.Login.Equals(login));
    }
}