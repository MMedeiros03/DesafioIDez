using DesafioIDez.Dominio.Entidades;

namespace DesafioIDez.Dominio.Interfaces.Providers;

public interface IIBGEProvider
{
    Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(string estado);
}
