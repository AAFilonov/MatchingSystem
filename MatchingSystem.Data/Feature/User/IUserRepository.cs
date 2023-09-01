namespace MatchingSystem.DataLayer.Feature.User;

public interface IUserRepository :IRepository<Data.Model.User>
{
    Data.Model.User? FindByLogin(string authLogin);
}