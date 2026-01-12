using DesafioIDez.Dominio.Entidades;

namespace DesafioIDez.Aplicacao.Interfaces.Servicos;

public interface IConsultaMunicipiosServico
{
    Task<List<Municipio>> ConsultarMunicipiosPorEstadoAsync(string estado);
}
