namespace MatchingSystem.DataLayer.Feature.Interface
{
    public interface ILoggerRepository
    {
        void LogRequest(string request, string endpoint);
    }
}
