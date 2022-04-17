namespace MatchingSystem.Data.Feature.User;

public interface IUserRepository :IRepository<Model.User>
{
    Model.User? findByLogin(string authLogin);
}