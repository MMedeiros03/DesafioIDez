using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;
using DesafioIDez.Infraestrutura.DTO;
using DesafioIDez.Infraestrutura.Excecoes;
using System.Text.Json;

namespace DesafioIDez.Infraestrutura.Providers;

public class IBGEProvider(HttpClient httpClient) : IIBGEProvider
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(string estado)
    {
        try
        {
            var resposta = await _httpClient.GetAsync($"v1/localidades/estados/{estado.ToLower()}/municipios");

            if (!resposta.IsSuccessStatusCode) throw new ServicoExternoException("Erro ao obter municípios da api IBGE.");

            var conteudo = await resposta.Content.ReadAsStringAsync();

            var opcoes = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var municipios = JsonSerializer.Deserialize<List<MunicipioIBGE>>(conteudo, opcoes);

            return municipios?
                .Select(m => new Municipio
                {
                    IBGE_Code = m.Id.ToString(),
                    Name = m.Nome
                }).ToList() ?? [];
        }
        catch (ServicoExternoException)
        {
            throw;
        }
        catch(Exception ex)
        {
            throw new Exception("Erro inesperado na comunicação com o servico IBGE.", ex);
        }
        
    }
}
