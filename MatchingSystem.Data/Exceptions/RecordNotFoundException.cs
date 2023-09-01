using System;

namespace MatchingSystem.DataLayer.Exceptions;

public class RecordNotFoundException : Exception
{
    public RecordNotFoundException(string msg) : base(msg)
    {
    }

    public  RecordNotFoundException()
    {
    }
}