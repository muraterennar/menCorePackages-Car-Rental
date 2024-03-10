namespace MenCore.CrossCuttingConserns.Exceptions.Types;

// BusinessException sınıfı, özel iş istisnaları için temel sınıf olarak kullanılır
public class BusinessException : Exception
{
    // Parametresiz kurucu metod
    public BusinessException ()
    {
    }

    // Bir mesajla kurucu metod
    public BusinessException (string? message) : base(message)
    {
    }

    // Bir mesaj ve iç istisna ile kurucu metod
    public BusinessException (string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
