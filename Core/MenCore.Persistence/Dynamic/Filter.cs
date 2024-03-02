namespace MenCore.Persistence.Dynamic;

#region Filter
// Bu sınıf, bir filtreleme işlemini temsil eder.
// Belirli bir alana, operatöre ve değere sahip filtreleme kriterlerini içerir.
// Mantıksal operatörleri ve bir dizi alt filtreyi destekler.
#endregion
public class Filter
{
    // Filtreleme kriterinin uygulanacağı alanı temsil eden özellik.
    public string Field { get; set; }

    // Filtre operatörünü temsil eden özellik.
    public string? Operator { get; set; }

    // Filtreleme kriterinin değerini temsil eden özellik.
    public string? Value { get; set; }

    // Filtreleme kriterleri arasındaki mantıksal ilişkiyi temsil eden özellik.
    public string? Logic { get; set; }

    // Alt filtrelerin koleksiyonunu temsil eden özellik.
    public IEnumerable<Filter>? Filters { get; set; }

    // Yapılandırıcı metot: Varsayılan değerlerle bir Filter örneği oluşturur.
    public Filter ()
    {
        Field = string.Empty;
        Operator = string.Empty;
    }

    // Yapılandırıcı metot: Belirli bir alan ve operatörle bir Filter örneği oluşturur.
    public Filter (string field, string @operator)
    {
        Field = field;
        Operator = @operator;
    }
}