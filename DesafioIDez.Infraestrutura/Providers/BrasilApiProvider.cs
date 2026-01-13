using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;
using DesafioIDez.Infraestrutura.DTO;
using System.Net.Http.Json;
using System.Text.Json;

namespace DesafioIDez.Infraestrutura.Providers;

public class BrasilApiProvider(HttpClient httpClient) : IBrasilApiProvider
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(string estado)
    {
        var resposta = await _httpClient.GetAsync($"ibge/municipios/v1/{estado}");

        if (!resposta.IsSuccessStatusCode) throw new Exception("Erro ao obter municípios do Brasil API.");

        var conteudo = await resposta.Content.ReadAsStringAsync();

        var opcoes = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var municipios = await resposta.Content.ReadFromJsonAsync<List<MunicioBrasilApiDTO>>();

        return municipios?
            .Select(m => new Municipio
            {
                IBGE_Code = m.Codigo_IBGE,
                Name = m.Nome
            }).ToList() ?? [];
    }
}
