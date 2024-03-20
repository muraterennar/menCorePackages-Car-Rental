using Serilog;

namespace MenCore.CrossCuttingConserns.Serilog;

// ILogger arayüzü kullanılarak günlükleme sağlayan temel bir sınıf
public abstract class LoggerServiceBase
{
    protected ILogger Logger { get; set; } // ILogger özelliği

    // Parametresiz kurucu metot, Logger özelliğine null atanır
    protected LoggerServiceBase()
    {
        Logger = null;
    }

    // ILogger örneği alan kurucu metot
    protected LoggerServiceBase(ILogger logger)
    {
        Logger = logger; // Logger özelliğine belirtilen ILogger atanır
    }

    // Loglama seviyesine bağlı olarak ilgili yöntemlerle mesaj yazdıran metotlar
    public void Verbose(string message) => Logger.Verbose(message);
    public void Fatal(string message) => Logger.Fatal(message);
    public void Info(string message) => Logger.Information(message);
    public void Warn(string message) => Logger.Warning(message);
    public void Debug(string message) => Logger.Debug(message);
    public void Error(string message) => Logger.Error(message);
}