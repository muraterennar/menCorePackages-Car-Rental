using MenCore.CrossCuttingConserns.Serilog.ConfigurationModels;
using MenCore.CrossCuttingConserns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace MenCore.CrossCuttingConserns.Serilog.Loggers;

// FileLogger sınıfı, dosya tabanlı günlükleme sağlayan bir logger servisini temsil eder
public class FileLogger : LoggerServiceBase
{
    private readonly IConfiguration _configuration; // Yapılandırma bilgileri

    // Yapılandırma bilgilerini parametre olarak alan kurucu metot
    public FileLogger(IConfiguration configuration)
    {
        _configuration = configuration; // Yapılandırma bilgileri atanır

        // Serilog yapılandırmasından dosya günlüğü yapılandırmasını alır
        var logConfig = configuration.GetSection("SerilogLogConfiguration:FileLogConfiguration")
                            .Get<FileLogConfiguration>()
                        ?? throw new Exception(SerilogMessages
                            .NullOptionsMessage); // Eğer yapılandırma null ise istisna fırlatılır

        // Günlük dosyasının yolu belirlenir
        var logFilePath = string.Format("{0}{1}", Directory.GetCurrentDirectory() + logConfig.FolderPath, ".txt");

        // Logger oluşturulur ve yapılandırılır
        Logger = new LoggerConfiguration().WriteTo.File(
            logFilePath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: null,
            fileSizeLimitBytes: 1000000,
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
        ).CreateLogger();
    }
}