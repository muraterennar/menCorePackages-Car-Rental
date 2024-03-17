namespace MenCore.Application.Pipelines.Caching;

// Bu arabirim, önbellekten kaldırma istekleri için gereken davranışları tanımlar.
public interface ICacheRemoverRequest
{
    // Önbellek anahtarını sağlar.
    // Bu anahtar, önbellekten kaldırılacak öğenin benzersiz kimliğini temsil eder.
    public string? CacheKey { get; }

    // Önbelleği atlayıp atlamama durumunu belirler.
    // true ise, önbelleği atlar; false ise, önbelleği kullanır.
    public bool BypassCache { get; }

    // Önbellek grubu anahtarını sağlar.
    // Bu anahtar, önbellekten kaldırılacak öğenin ait olduğu grubu temsil eder.
    public string? CacheGroupKey { get; }
}
