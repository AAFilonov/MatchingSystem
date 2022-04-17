namespace MatchingSystem.DataLayer.Interface
{
    public interface ILoggerRepository
    {
        void LogRequest(string request, string endpoint);
    }
}
