using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;

namespace DesafioIDez.Aplicacao.Servicos;

public class ConsultaMunicipiosBrasilApi(IBrasilApiProvider brasilApiProvider ) : IConsultaMunicipiosServico
{
    private readonly IBrasilApiProvider _brasilApiProvider = brasilApiProvider;

    public async Task<List<Municipio>> ConsultarMunicipiosPorEstadoAsync(string estado)
    {
        return await _brasilApiProvider.ObterMunicipiosPorEstadoAsync(estado);
    }
}
