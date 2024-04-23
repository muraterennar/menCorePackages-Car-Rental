namespace MenCore.CrossCuttingConserns.Exceptions.Types;

public class TransactionException : Exception
{
    public TransactionException()
    {
    }

    public TransactionException(string? message) : base(message)
    {
    }

    public TransactionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}