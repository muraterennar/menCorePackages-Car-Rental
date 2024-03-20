namespace MenCore.CrossCuttingConserns.Logging;

// LogDetail sınıfı, bir log detayının tamamını temsil eder
public class LogDetail
{
    public string FullName { get; set; } // Tam isim
    public string MethodName { get; set; } // Metodun adı
    public string User { get; set; } // Kullanıcı
    public List<LogParameter> Parameters { get; set; } // Parametrelerin listesi

    // Parametresiz kurucu metot, varsayılan değerler atanır
    public LogDetail()
    {
        FullName = string.Empty; // Boş bir dize atanır
        MethodName = string.Empty; // Boş bir dize atanır
        User = string.Empty; // Boş bir dize atanır
        Parameters = new List<LogParameter>(); // Boş bir parametre listesi oluşturulur
    }

    // Tam isim, metod adı, kullanıcı ve parametrelerle kurucu metot
    public LogDetail(string fullName, string methodName, string user, List<LogParameter> parameters)
    {
        FullName = fullName; // Tam isim atanır
        MethodName = methodName; // Metod adı atanır
        User = user; // Kullanıcı atanır
        Parameters = parameters; // Parametreler atanır
    }
}
