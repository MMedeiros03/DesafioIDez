
using DesafioIDez.Dominio.Entidades;

namespace DesafioIDez.Dominio.Interfaces.Providers;

public interface IBrasilApiProvider
{
    Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(string estado);
}
