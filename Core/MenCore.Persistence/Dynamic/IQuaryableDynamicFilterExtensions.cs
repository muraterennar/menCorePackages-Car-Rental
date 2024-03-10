using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace MenCore.Persistence.Dynamic;

public static class IQuaryableDynamicFilterExtensions
{
    #region İlk Hazırlık

    // Filtre, sıralama ve mantıksal işlemler için dizileri ve sözlüğü başlat
    private static readonly string[] _orders = { "asc", "desc" };
    private static readonly string[] _logics = { "and", "or" };

    private static readonly IDictionary<string, string> _operators = new Dictionary<string, string>
        {
            { "eq", "=" },
            { "neq", "!=" },
            { "lt", "<" },
            { "lte", "<=" },
            { "gt", ">" },
            { "gte", ">=" },
            { "isnull", "== null" },
            { "isnotnull", "!= null" },
            { "startswith", "StartsWith" },
            { "endswith", "EndsWith" },
            { "contains", "Contains" },
            { "doesnotcontain", "Contains" }
        };

    #endregion

    #region Ana Yöntem: ToDynamic

    // Dinamik filtreleme ve sıralama uygulamak için genişletme yöntemi
    public static IQueryable<T> ToDynamic<T> (this IQueryable<T> query, DynamicQuery dynamicQuery)
    {
        // Filtre varsa uygula
        if (dynamicQuery.Filter is not null)
            query = Filter(query, dynamicQuery.Filter);

        // Sıralama varsa uygula
        if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())
            query = Sort(query, dynamicQuery.Sort);

        return query;
    }

    #endregion

    #region Filtreleme

    // IQueryable'e filtre uygulamak için yöntem
    private static IQueryable<T> Filter<T> (IQueryable<T> queryable, Filter filter)
    {
        // Tüm filtreleri düzleştirilmiş bir liste olarak al
        IList<Filter> filters = GetAllFilters(filter);

        // Filtrelerden değerleri çıkar
        string?[] values = filters.Select(f => f.Value).ToArray();

        // Filtreleri dinamik bir where ifadesine dönüştür
        string where = Transform(filter, filters);

        // Dinamik where ifadesini queryable'a uygula
        if (!string.IsNullOrEmpty(where) && values != null)
            queryable = queryable.Where(where, values);

        return queryable;
    }

    // Tüm filtreleri içeren bir liste almak için özyinelemeli yöntem
    public static IList<Filter> GetAllFilters (Filter filter)
    {
        List<Filter> filters = new();
        GetFilters(filter, filters);
        return filters;
    }

    // Tüm filtreleri toplamak için özyinelemeli yöntem
    private static void GetFilters (Filter filter, IList<Filter> filters)
    {
        filters.Add(filter);
        if (filter.Filters is not null && filter.Filters.Any())
            foreach (Filter item in filter.Filters)
                GetFilters(item, filters);
    }

    #endregion

    #region Sıralama

    // IQueryable'e verilen sıralama kriterlerine göre sıralama uygulamak için yöntem
    private static IQueryable<T> Sort<T> (IQueryable<T> queryable, IEnumerable<Sort> sort)
    {
        foreach (Sort item in sort)
        {
            if (string.IsNullOrEmpty(item.Field))
                throw new ArgumentException("Geçersiz Alan");
            if (string.IsNullOrEmpty(item.Dir) || !_orders.Contains(item.Dir))
                throw new ArgumentException("Geçersiz Sıralama Türü");
        }

        // Sıralama kriterlerine göre sıralama dizgesi oluştur
        if (sort.Any())
        {
            string ordering = string.Join(separator: ",", values: sort.Select(s => $"{s.Field} {s.Dir}"));
            return queryable.OrderBy(ordering);
        }

        return queryable;
    }

    #endregion

    #region Dönüşüm

    // Filtre nesnesini dinamik where ifadesine dönüştürmek için yöntem
    public static string Transform (Filter filter, IList<Filter> filters)
    {
        if (string.IsNullOrEmpty(filter.Field))
            throw new ArgumentException("Geçersiz Alan");
        if (string.IsNullOrEmpty(filter.Operator) || !_operators.ContainsKey(filter.Operator))
            throw new ArgumentException("Geçersiz Operatör");

        int index = filters.IndexOf(filter);
        string comparison = _operators[filter.Operator];
        StringBuilder where = new();

        if (!string.IsNullOrEmpty(filter.Value))
            if (filter.Operator == "doesnotcontain")
                where.Append($"(!np({filter.Field}).{comparison}(@{index.ToString()}))");
            else if (comparison is "StartsWith" or "EndsWith" or "Contains")
                where.Append($"(np({filter.Field}).{comparison}(@{index.ToString()}))");
            else
                where.Append($"np({filter.Field}) {comparison} @{index.ToString()}");
        else if (filter.Operator is "isnull" or "isnotnull")
            where.Append($"np({filter.Field}) {comparison}");

        // İç içe geçmiş filtreler için mantıksal operatörleri işle
        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
        {
            if (!_logics.Contains(filter.Logic))
                throw new ArgumentException("Geçersiz Mantıksal Operatör");

            // İç içe geçmiş filtreleri özyinelemeli olarak dönüştür ve mantıksal operatörlerle birleştir
            return $"{where} {filter.Logic} ({string.Join(separator: $" {filter.Logic} ", value: filter.Filters.Select(f => Transform(f, filters)).ToArray())})";
        }

        return where.ToString();
    }

    #endregion
}