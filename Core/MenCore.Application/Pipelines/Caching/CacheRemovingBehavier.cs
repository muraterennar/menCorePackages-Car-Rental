using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace MenCore.Application.Pipelines.Caching;

public class CacheRemovingBehavier<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheRemoverRequest
{
    private readonly IDistributedCache _cache;

    public CacheRemovingBehavier(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Önbelleği atlamak isteniyorsa, işlemi devam ettirir.
        if (request.BypassCache)
            return await next();

        // İsteği işler.
        TResponse response = await next();

        // Önbellek grubu anahtarı varsa, önbellekteki öğeleri kaldırır.
        if (request.CacheGroupKey != null)
        {
            // Önbellek grubunu alır.
            byte[]? cacheGroup = await _cache.GetAsync(request.CacheGroupKey, cancellationToken);
            if (cacheGroup != null)
            {
                // Önbellek grubunu deserialize eder.
                HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroup));
                foreach (var key in keysInGroup)
                {
                    // Önbellek grubundaki her öğeyi kaldırır.
                    await _cache.RemoveAsync(key, cancellationToken);
                }

                // Önbellek grubunu kaldırır.
                await _cache.RemoveAsync(request.CacheGroupKey, cancellationToken);

                // Önbellek grubunun süresini tutan anahtarı kaldırır.
                await _cache.RemoveAsync(key: $"{request.CacheGroupKey}SlidingExpiration", cancellationToken);
            }
        }

        // Önbellek anahtarı varsa, önbellekteki öğeyi kaldırır.
        if (request.CacheKey != null)
        {
            await _cache.RemoveAsync(request.CacheKey, cancellationToken);
        }

        return response;
    }
}