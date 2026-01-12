using DesafioIDez.Aplicacao.Enums;
using Microsoft.Extensions.DependencyInjection;
using DesafioIDez.Aplicacao.Interfaces.Factory;
using DesafioIDez.Aplicacao.Interfaces.Servicos;

namespace DesafioIDez.Aplicacao.Factories;

public class ConsultaMunicipiosFactory(IServiceProvider serviceProvider) : IConsultaMunicipiosFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IConsultaMunicipiosServico ObterConsulta(string provider)
    {
        if (!Enum.TryParse<ProviderEnum>(provider, true, out var providerEnum))
            throw new InvalidOperationException("Provider inválido ou não configurado.");

        return _serviceProvider.GetKeyedService<IConsultaMunicipiosServico>(providerEnum)
            ?? throw new NotImplementedException(
                $"O provedor {provider} não está implementado.");
    }
}
