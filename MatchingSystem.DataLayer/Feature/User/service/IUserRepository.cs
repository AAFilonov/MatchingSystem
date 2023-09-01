namespace MatchingSystem.DataLayer.Feature.User;

public interface IUserRepository :IRepository<User>
{
    User? getByLogin(string authLogin);
}