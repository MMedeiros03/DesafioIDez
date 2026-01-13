using DesafioIDez.Aplicacao.DTO;
using DesafioIDez.Dominio.Entidades;

namespace DesafioIDez.Aplicacao.Interfaces.Servicos;

public interface IConsultaMunicipiosServico
{
    Task<ListaPaginadaRDTO<Municipio>> ConsultarMunicipiosPorEstadoAsync(FiltroEstadoDto filtroEstado);
}
