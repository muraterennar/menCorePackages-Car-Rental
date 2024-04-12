using System.Runtime.Serialization;

namespace MenCore.CrossCuttingConserns.Exceptions.Types;

public class AuthorizationException : Exception
{
    // Parametresiz yapıcı metot.
    public AuthorizationException() { }

    // Serileştirme bilgileri ile korumalı yapıcı metot.
    protected AuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    // Mesajı belirterek yapıcı metot.
    public AuthorizationException(string? message) : base(message) { }

    // Mesaj ve iç içe istisna belirterek yapıcı metot.
    public AuthorizationException(string? message, Exception? innerException) : base(message, innerException) { }
}
