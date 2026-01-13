using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;

namespace DesafioIDez.Aplicacao.Servicos;

public class ConsultaMunicipiosIBGE(IIBGEProvider ibgeProvider) : IConsultaMunicipiosServico
{
    private readonly IIBGEProvider _ibgeProvider = ibgeProvider;

    public async Task<List<Municipio>> ConsultarMunicipiosPorEstadoAsync(string estado)
    {
        return await _ibgeProvider.ObterMunicipiosPorEstadoAsync(estado);
    }
}
