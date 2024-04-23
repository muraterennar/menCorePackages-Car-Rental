namespace MenCore.CrossCuttingConserns.Serilog.ConfigurationModels;

// FileLogConfiguration sınıfı, dosya tabanlı günlük oluşturma yapılandırmasını temsil eder
public class FileLogConfiguration
{
    // Parametresiz kurucu metot, varsayılan değerler atanır
    public FileLogConfiguration()
    {
        FolderPath = string.Empty; // Boş bir dize atanır
    }

    // Klasör yolunu parametre olarak alan kurucu metot
    public FileLogConfiguration(string folderPath)
    {
        FolderPath = folderPath; // Klasör yoluna değer atanır
    }

    public string FolderPath { get; set; } // Dosyaların kaydedileceği klasör yolu
}