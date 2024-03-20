namespace MenCore.CrossCuttingConserns.Logging;

// LogDetailWithException sınıfı, istisna mesajı ile birlikte bir log detayını temsil eder
public class LogDetailWithException : LogDetail
{
    public string ExceptionMessage { get; set; } // İstisna mesajı

    // Parametresiz kurucu metot, varsayılan değerler atanır
    public LogDetailWithException()
    {
        ExceptionMessage = string.Empty; // Boş bir dize atanır
    }

    // İstisna mesajı ile kurucu metot
    public LogDetailWithException(string exceptionMessage)
    {
        ExceptionMessage = exceptionMessage; // İstisna mesajı atanır
    }

    // Tam isim, metod adı, kullanıcı, istisna mesajı ve parametrelerle kurucu metot
    public LogDetailWithException(string fullName, string methodName, string user, string exceptionMessage, List<LogParameter> parameters)
    {
        FullName = fullName; // Tam isim atanır
        MethodName = methodName; // Metod adı atanır
        User = user; // Kullanıcı atanır
        ExceptionMessage = exceptionMessage; // İstisna mesajı atanır
        Parameters = parameters; // Parametreler atanır
    }
}
