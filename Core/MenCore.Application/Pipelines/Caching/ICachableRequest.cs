namespace MenCore.Application.Pipelines.Caching;

// Bu arabirim, önbelleğe alınabilir istekler için gereken davranışları tanımlar.
public interface ICachableRequest
{
    // Önbellek grubu anahtarını sağlar.
    // Bu anahtar, önbellekteki öğenin ait olduğu grubu temsil eder.
    public string CacheGroupKey { get; }

    // Önbellek anahtarını sağlar.
    // Bu anahtar, önbellekteki öğenin benzersiz kimliğini temsil eder.
    public string CacheKey { get; }

    // Önbelleği atlayıp atlamama durumunu belirler.
    // true ise, önbelleği atlar; false ise, önbelleği kullanır.
    public bool BypassCache { get; }

    // Kayıtların önbellekte ne kadar süreyle saklanacağını belirtir (isteğe bağlı).
    // Zaman aşımı süresi, son kullanımdan itibaren geçen süreyi temsil eder.
    // Örneğin, SlidingExpiration değeri 60 ise, bir öğe son kullanıldıktan 60 saniye sonra önbellekten kaldırılır.
    public TimeSpan? SlidingExpiration { get; }
}