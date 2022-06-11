namespace MatchingSystem.Service.Exception;

public class InputDataException : System.Exception
{
    public InputDataException(string msg) : base(msg)
    {
    }

    public  InputDataException()
    {
    }

    public InputDataException(string msg, System.Exception e) : base(msg, e)
    {
      
    }
}