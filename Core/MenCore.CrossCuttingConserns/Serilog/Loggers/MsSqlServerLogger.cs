using MenCore.CrossCuttingConserns.Serilog.ConfigurationModels;
using MenCore.CrossCuttingConserns.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace MenCore.CrossCuttingConserns.Serilog.Loggers;

public class MsSqlServerLogger : LoggerServiceBase
{
    private readonly IConfiguration _configuration;

    public MsSqlServerLogger(IConfiguration configuration)
    {
        _configuration = configuration; // IConfiguration nesnesi atanır

        // Serilog yapılandırmasından MsSqlServerConfiguration örneğini alır
        MsSqlServerConfiguration logConfig = configuration.GetSection("SerilogLogConfiguration:ServerLogConfiguration:MsSqlConfiguration").Get<MsSqlServerConfiguration>()
            ?? throw new Exception(SerilogMessages.NullOptionsMessage); // Eğer yapılandırma null ise istisna fırlatılır

        // MSSqlServerSinkOptions ve ColumnOptions örnekleri oluşturulur ve uygun değerler atanır
        MSSqlServerSinkOptions sinkOptions = new()
        {
            TableName = logConfig.TableName, // Tablo adı atanır
            AutoCreateSqlTable = logConfig.AutoCreateSqlTable // Otomatik tablo oluşturma durumu atanır
        };

        ColumnOptions columnOptions = new();

        // Serilog yapılandırması oluşturulur ve MSSqlServer sink kullanılır
        global::Serilog.Core.Logger seriLogConfig = new LoggerConfiguration().WriteTo.MSSqlServer(
            connectionString: logConfig.ConnectionString, // Bağlantı dizesi atanır
            sinkOptions: sinkOptions, // Sink seçenekleri atanır
            columnOptions: columnOptions // Sütun seçenekleri atanır
            ).CreateLogger();

        Logger = seriLogConfig; // Logger nesnesi atanır
    }
}