namespace MenCore.Application.Pipelines.Caching;

// Bu sınıf, önbellekleme ayarlarını tutar.
public class CacheSettings
{
    // Kayıtların önbellekte ne kadar süreyle saklanacağını belirler.
    // Zaman aşımı süresi, son kullanımdan itibaren geçen süre olarak ayarlanır.
    // Örneğin, SlidingExpiration değeri 60 ise, bir öğe son kullanıldıktan 60 saniye sonra önbellekten kaldırılır.
    public int SlidingExpiration { get; set; }
}
