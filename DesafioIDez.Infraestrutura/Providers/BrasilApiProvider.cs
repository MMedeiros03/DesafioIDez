using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;
using DesafioIDez.Infraestrutura.DTO;
using DesafioIDez.Infraestrutura.Excecoes;
using System.Net.Http.Json;
using System.Text.Json;

namespace DesafioIDez.Infraestrutura.Providers;

public class BrasilApiProvider(HttpClient httpClient) : IBrasilApiProvider
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(string estado)
    {
        try
        {
            var resposta = await _httpClient.GetAsync($"ibge/municipios/v1/{estado}");

            if (!resposta.IsSuccessStatusCode) throw new ServicoExternoException("Houve um erro ao obter municípios do Brasil API.");

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
        catch(ServicoExternoException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado na comunicação com o BrasilApi.", ex);
        }
    }
}
