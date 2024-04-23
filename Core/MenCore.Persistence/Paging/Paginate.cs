namespace MenCore.Persistence.Paging;

#region Paginate

// Bu sınıf, sayfalama (pagination) işlevselliğini sağlar.
// Sayfalama işlemleri için gerekli olan özellikleri ve davranışları içerir.

#endregion

public class Paginate<T>
{
    // Sınıfın yapılandırıcı metodu.
    // Sayfa koleksiyonunu başlangıçta boş bir koleksiyonla başlatır.
    public Paginate()
    {
        Items = Array.Empty<T>();
    }

    // Her sayfa boyutunu (kaç öğe gösterileceği) belirten özellik.
    public int Size { get; set; }

    // Geçerli sayfanın indeksini belirten özellik.
    public int Index { get; set; }

    // Toplam öğe sayısını belirten özellik.
    public int Count { get; set; }

    // Toplam sayfa sayısını belirten özellik.
    public int Pages { get; set; }

    // Sayfadaki öğeleri temsil eden koleksiyon özelliği.
    public IList<T> Items { get; set; }

    // Önceki sayfa var mı kontrol eden özellik.
    public bool HasPrevious => Index > 0;

    // Sonraki sayfa var mı kontrol eden özellik.
    public bool HasNext => Index + 1 < Pages;
}