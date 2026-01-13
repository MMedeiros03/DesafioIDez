namespace DesafioIDez.Aplicacao.Interfaces.Servicos;

public interface IRedisCacheServico
{
    Task<T?> ObterAsync<T>(string key);
    Task AdicionarAsync<T>(string key, T value, TimeSpan expiration);
}
