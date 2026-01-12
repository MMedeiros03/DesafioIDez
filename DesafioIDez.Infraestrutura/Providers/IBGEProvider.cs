using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;
using DesafioIDez.Infraestrutura.DTO;
using System.Text.Json;

namespace DesafioIDez.Infraestrutura.Providers;

public class IBGEProvider(HttpClient httpClient) : IIBGEProvider
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(string estado)
    {
        var resposta = await _httpClient.GetAsync($"v1/localidades/estados/{estado.ToLower()}/municipios");

        if (!resposta.IsSuccessStatusCode) throw new Exception("Erro ao obter municípios do Brasil API.");

        var conteudo = await resposta.Content.ReadAsStringAsync();

        var opcoes = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var municipios = JsonSerializer.Deserialize<List<MunicioBrasilApiDTO>>(conteudo, opcoes);

        return municipios?
            .Select(m => new Municipio
            {
                Codigo_IBGE = m.Codigo_IBGE,
                Nome = m.Nome
            }).ToList() ?? [];
    }
}
