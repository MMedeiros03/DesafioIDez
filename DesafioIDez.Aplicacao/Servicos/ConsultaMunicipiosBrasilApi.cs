using DesafioIDez.Aplicacao.DTO;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;

namespace DesafioIDez.Aplicacao.Servicos;

public class ConsultaMunicipiosBrasilApi(IBrasilApiProvider brasilApiProvider, IRedisCacheServico redisCacheServico)
    : ConsultaMunicipioBase(redisCacheServico), IConsultaMunicipiosServico
{
    private readonly IBrasilApiProvider _brasilApiProvider = brasilApiProvider;

    public Task<ListaPaginadaRDTO<Municipio>> ConsultarMunicipiosPorEstadoAsync(FiltroEstadoDto filtroEstado)
    {
        return ConsultarAsync(
            filtroEstado.Estado,
            filtroEstado,
            "desafioidez:brasilApi:municipios",
            _brasilApiProvider.ObterMunicipiosPorEstadoAsync);
    }
}
