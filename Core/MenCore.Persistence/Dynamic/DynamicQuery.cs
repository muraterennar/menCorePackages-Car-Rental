namespace MenCore.Persistence.Dynamic;

#region DynamicQuery
// Bu sınıf, dinamik sorguları temsil eder.
// Sıralama ve filtreleme kriterlerini içerebilir.
#endregion
public class DynamicQuery
{
    // Sıralama kriterlerinin koleksiyonunu temsil eden özellik.
    public IEnumerable<Sort>? Sort { get; set; }

    // Filtreleme kriterini temsil eden özellik.
    public Filter? Filter { get; set; }

    // Yapılandırıcı metot: Varsayılan değerlerle bir DynamicQuery örneği oluşturur.
    public DynamicQuery ()
    {

    }

    // Yapılandırıcı metot: Belirli sıralama ve filtreleme kriterleriyle bir DynamicQuery örneği oluşturur.
    public DynamicQuery (IEnumerable<Sort>? sort, Filter? filter)
    {
        Sort = sort;
        Filter = filter;
    }
}