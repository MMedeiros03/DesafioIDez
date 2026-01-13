using DesafioIDez.Aplicacao.Interfaces.Servicos;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DesafioIDez.Infraestrutura.Cache
{
    public class RedisCacheServico(IDistributedCache distributedCache) : IRedisCacheServico
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> ObterAsync<T>(string key)
        {
            var cachedValue = await _distributedCache.GetStringAsync(key);
            return cachedValue is null
                ? default
                : JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task AdicionarAsync<T>(string key, T value, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var json = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, json, options);
        }
    }
}
