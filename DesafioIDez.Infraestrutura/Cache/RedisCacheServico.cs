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
            try
            {
                var cachedValue = await _distributedCache.GetStringAsync(key);
                return cachedValue is null
                    ? default
                    : JsonSerializer.Deserialize<T>(cachedValue);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro de comunicação com o Redis ao tentar buscar valores.", ex);
            }
        }

        public async Task AdicionarAsync<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                };

                var json = JsonSerializer.Serialize(value);
                await _distributedCache.SetStringAsync(key, json, options);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro de comunicação com o Redis ao tentar registrar valores.", ex);
            }
        }
    }
}
