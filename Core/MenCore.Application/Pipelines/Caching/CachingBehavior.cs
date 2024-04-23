using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace MenCore.Application.Pipelines.Caching;

// Bu sınıf, önbellekleme davranışını uygular.
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachableRequest
{
    private readonly IDistributedCache _cache;
    private readonly CacheSettings _cacheSettings;

    public CachingBehavior(IDistributedCache cache, IConfiguration configuration)
    {
        _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ??
                         throw new InvalidOperationException();
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Önbelleği atlamak isteniyorsa, işlemi devam ettirir.
        if (request.BypassCache)
            return await next();

        TResponse response;
        // Önbellekteki yanıtı alır.
        var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);
        if (cachedResponse != null)
            // Önbellekteki yanıtı deserialize eder.
            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
        else
            // Önbellekte yanıt yoksa, yanıtı alır ve önbelleğe koyar.
            response = await GetResponseAndCache(request, next, cancellationToken);

        return response;
    }


    #region GetResponseAndCache

    private async Task<TResponse> GetResponseAndCache(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // İstenen yanıtı alır.
        var response = await next();

        // Kayıtların önbellekte ne kadar süreyle saklanacağını belirler.
        var slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);

        // Önbellek seçeneklerini belirler.
        DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };

        // Yanıtı serialize eder.
        var serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

        // Yanıtı önbelleğe koyar.
        await _cache.SetAsync(request.CacheKey, serializedData, cacheOptions, cancellationToken);

        if (request.CacheGroupKey != null)
            await AddCacheKeyToGroup(request, slidingExpiration, cancellationToken);

        return response;
    }

    #endregion

    #region AddCacheKeyToGroup

    private async Task AddCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration,
        CancellationToken cancellationToken)
    {
        // Önbellek grubuna eklenen önbellek anahtarlarını içeren küme.
        HashSet<string> cacheKeysInGroup;

        // Önbellek grubu için mevcut önbellek verisini alır.
        var cacheGroupCache = await _cache.GetAsync(request.CacheGroupKey!, cancellationToken);

        if (cacheGroupCache != null)
        {
            // Mevcut önbellek verisini deserialize eder.
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache));

            // Yeni önbellek anahtarını önbellek grubuna ekler.
            if (!cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
        }
        else
        {
            // Önbellek grubu için önbellek verisi bulunamadığı durumda yeni bir küme oluşturur.
            cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });
        }

        // Önbellek grubu verisini serialize eder.
        var newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

        // Önbellek grubu için kayıtların önbellekte ne kadar süreyle saklanacağını belirler.
        var cacheGroupCacheSlidingExpirationCache = await _cache.GetAsync(
            $"{request.CacheGroupKey}SlidingExpiration",
            cancellationToken
        );

        int? cacheGroupCacheSlidingExpirationValue = null;
        if (cacheGroupCacheSlidingExpirationCache != null)
            cacheGroupCacheSlidingExpirationValue =
                Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));

        // Önbellek grubu için mevcut süre değerini günceller.
        if (cacheGroupCacheSlidingExpirationValue == null ||
            slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);

        // Yeni süre değerini serialize eder.
        var serializeCacheGroupSlidingExpirationData =
            JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

        // Önbellek seçeneklerini belirler.
        DistributedCacheEntryOptions cacheOptions = new()
            { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

        // Önbellek grubu verisini günceller.
        await _cache.SetAsync(request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);

        // Önbellek grubu için kayıtların önbellekteki süresini günceller.
        await _cache.SetAsync(
            $"{request.CacheGroupKey}SlidingExpiration",
            serializeCacheGroupSlidingExpirationData,
            cacheOptions,
            cancellationToken
        );
    }

    #endregion
}