using DesafioIDez.Aplicacao.DTO;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Dominio.Entidades;
using System;

namespace DesafioIDez.Aplicacao.Servicos;

public abstract class ConsultaMunicipioBase(IRedisCacheServico redisCacheServico)
{
    private readonly IRedisCacheServico _redisCacheServico = redisCacheServico;

    protected async Task<ListaPaginadaRDTO<Municipio>> ConsultarAsync(
        string estado,
        FiltroEstadoDto filtro,
        string cachePrefix,
        Func<string, Task<List<Municipio>>> obterDadosAsync)
    {
        var uf = estado.ToUpperInvariant();
        var cacheKey = $"{cachePrefix}:uf:{uf}";

        var dadosCache = await _redisCacheServico.ObterAsync<List<Municipio>>(cacheKey);

        var municipios = dadosCache ?? await ObterECachearAsync(cacheKey, uf, obterDadosAsync);

        return CriarListaPaginada(municipios, filtro);
    }

    private async Task<List<Municipio>> ObterECachearAsync(
        string cacheKey,
        string estado,
        Func<string, Task<List<Municipio>>> obterDadosAsync)
    {
        var dados = await obterDadosAsync(estado);
        await _redisCacheServico.AdicionarAsync(cacheKey, dados, TimeSpan.FromHours(2));
        return dados;
    }

    private static ListaPaginadaRDTO<Municipio> CriarListaPaginada(List<Municipio> municipios, FiltroEstadoDto filtroEstado)
    {
        var pagina = filtroEstado.Pagina <= 0 ? 1 : filtroEstado.Pagina;
        var tamanhoPagina = filtroEstado.TamanhoPagina <= 0 ? 10 : filtroEstado.TamanhoPagina;

        var itensFiltrados = municipios
            .Where(m =>
                (string.IsNullOrEmpty(filtroEstado.Municipio) || m.Name.StartsWith(filtroEstado.Municipio)) &&
                (string.IsNullOrEmpty(filtroEstado.Codigo_Ibge) || m.IBGE_Code.StartsWith(filtroEstado.Codigo_Ibge))
            ).ToList();
        
        var itensPaginados = itensFiltrados
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToList();

        return new ListaPaginadaRDTO<Municipio>
        {
            Pagina = filtroEstado.Pagina,
            TamanhoPagina = tamanhoPagina,
            Itens = itensPaginados,
            TotalItens = itensFiltrados.Count,
            TotalPaginas = (int)Math.Ceiling((double)itensFiltrados.Count / tamanhoPagina)
        };
    }
}
