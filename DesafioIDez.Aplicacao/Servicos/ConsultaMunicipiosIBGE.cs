using DesafioIDez.Aplicacao.DTO;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;

namespace DesafioIDez.Aplicacao.Servicos;

public class ConsultaMunicipiosIBGE(
        IIBGEProvider ibgeProvider,
        IRedisCacheServico redisCacheServico) : ConsultaMunicipioBase(redisCacheServico), IConsultaMunicipiosServico
{
    private readonly IIBGEProvider _ibgeProvider = ibgeProvider;

    public Task<ListaPaginadaRDTO<Municipio>> ConsultarMunicipiosPorEstadoAsync(FiltroEstadoDto filtroEstado)
    {
        return ConsultarAsync(
            filtroEstado.Estado,
            filtroEstado,
            "desafioidez:ibge:municipios",
            _ibgeProvider.ObterMunicipiosPorEstadoAsync);
    }
}
