using Microsoft.EntityFrameworkCore;

namespace MenCore.Persistence.Paging;

public static class IQuaryablePaginateExtensions
{
    public static async Task<Paginate<T>> ToPaginateAsync<T>(
        this IQueryable<T> source,
        int index,
        int size,
        CancellationToken cancellationToken)
    {
        // Kaynak üzerindeki toplam öğe sayısını alır.
        var count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

        // Belirtilen indeksten başlayarak belirli bir boyutta öğeleri alır ve listeye dönüştürür.
        var items = await source.Skip(index * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

        // Paginate örneği oluşturur ve özelliklerini ayarlar.
        Paginate<T> list = new()
        {
            Index = index,
            Count = count,
            Items = items,
            Size = size,
            Pages = (int)Math.Ceiling(count / (double)size)
        };

        return list;
    }

    public static Paginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size, int from = 0)
    {
        // Başlangıç indeksi kontrol edilir.
        if (from > index)
            throw new ArgumentException($"From: {from} > Index: {index}, must from <= Index");

        // Kaynak üzerindeki toplam öğe sayısını alır.
        var count = source.Count();

        // Belirtilen indeksten başlayarak belirli bir boyutta öğeleri alır ve listeye dönüştürür.
        var items = source.Skip((index - from) * size).Take(size).ToList();

        // Paginate örneği oluşturur ve özelliklerini ayarlar.
        Paginate<T> list = new()
        {
            Index = index,
            Size = size,
            Count = count,
            Items = items,
            Pages = (int)Math.Ceiling(count / (double)size)
        };

        return list;
    }

    #region ToPaginateAsync

    // Bu genişletme (extension) metodu, asenkron olarak bir IQueryable kaynağını belirli bir indeks ve boyutla sayfalara böler.
    // İşlem iptal edilmek istenirse cancellationToken kullanılır.
    // Sonuç, bir Task içinde bir Paginate örneği olarak döndürülür.

    #endregion

    #region ToPaginate

    // Bu genişletme (extension) metodu, bir IQueryable kaynağını belirli bir indeks ve boyutla sayfalara böler.
    // Başlangıç indeksi from parametresi ile belirlenebilir.
    // Sonuç, bir Paginate örneği olarak döndürülür.

    #endregion
}