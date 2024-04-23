namespace MenCore.CrossCuttingConserns.Logging;

// LogParameter sınıfı, bir parametrenin adı, değeri ve türüyle ilgili bilgileri içerir
public class LogParameter
{
    // Parametresiz kurucu metot, varsayılan değerler atanır
    public LogParameter()
    {
        Name = string.Empty; // Boş bir dize atanır
        Value = string.Empty; // Boş bir dize atanır
        Type = string.Empty; // Boş bir dize atanır
    }

    // Ad, değer ve tür bilgileriyle kurucu metot
    public LogParameter(string name, object value, string type)
    {
        Name = name; // Ad atanır
        Value = value; // Değer atanır
        Type = type; // Tür atanır
    }

    public string Name { get; set; } // Parametrenin adı
    public object Value { get; set; } // Parametrenin değeri
    public string Type { get; set; } // Parametrenin türü
}