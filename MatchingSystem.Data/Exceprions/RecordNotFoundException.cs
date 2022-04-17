namespace MatchingSystem.Data.Exceprions;

public class RecordNotFoundException : Exception
{
    public RecordNotFoundException(string msg) : base(msg)
    {
    }

    public  RecordNotFoundException()
    {
    }
}