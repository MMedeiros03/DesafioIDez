using DesafioIDez.Aplicacao.Interfaces.Servicos;

namespace DesafioIDez.Aplicacao.Interfaces.Factory;

public interface IConsultaMunicipiosFactory
{
    IConsultaMunicipiosServico ObterConsulta(string provider);
}
