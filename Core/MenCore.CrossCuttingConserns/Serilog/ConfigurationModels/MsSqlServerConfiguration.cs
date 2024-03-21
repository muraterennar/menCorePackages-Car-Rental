namespace MenCore.CrossCuttingConserns.Serilog.ConfigurationModels;

public class MsSqlServerConfiguration
{
    public string ConnectionString { get; set; } // SQL Server bağlantı dizesi
    public string TableName { get; set; } // Günlük tablosunun adı
    public bool AutoCreateSqlTable { get; set; } // Otomatik olarak SQL tablosu oluşturulacak mı?

    // Parametresiz kurucu metot, varsayılan değerler atanır
    public MsSqlServerConfiguration()
    {
        ConnectionString = string.Empty; // Boş bir bağlantı dizesi atanır
        TableName = string.Empty; // Boş bir tablo adı atanır
        AutoCreateSqlTable = true; // Otomatik tablo oluşturma varsayılan olarak etkinleştirilir
    }

    // Bağlantı dizesi, tablo adı ve otomatik tablo oluşturma durumuyla kurucu metot
    public MsSqlServerConfiguration(string connectionString, string tableName, bool autoCreateSqlTable)
    {
        ConnectionString = connectionString; // Bağlantı dizesi atanır
        TableName = tableName; // Tablo adı atanır
        AutoCreateSqlTable = autoCreateSqlTable; // Otomatik tablo oluşturma durumu atanır
    }
}