using Microsoft.EntityFrameworkCore;

namespace MatchingSystem.Data.Feature.User;

public class UserRepository :Repository<Model.User>,IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }

    public Model.User? findByLogin(string login)
    {
        return _matchingSystemContext.Users.Single(user => user.Login.Equals(login));
    }
}