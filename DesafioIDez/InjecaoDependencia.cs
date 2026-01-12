using DesafioIDez.Aplicacao.Enums;
using DesafioIDez.Aplicacao.Factories;
using DesafioIDez.Aplicacao.Interfaces.Factory;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Dominio.Interfaces.Providers;
using DesafioIDez.Infraestrutura.Providers;
using DesafioIDez.Infraestrutura.Servicos;

namespace DesafioIDez.Api;

public static class InjecaoDependencia
{
    public static void AdicionarDependencias(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConsultaMunicipiosFactory, ConsultaMunicipiosFactory>();
        builder.Services.AddKeyedScoped<IConsultaMunicipiosServico, ConsultaMunicipiosBrasilApi>(ProviderEnum.BrasilApi);
        builder.Services.AddKeyedScoped<IConsultaMunicipiosServico, ConsultaMunicipiosIBGE>(ProviderEnum.IBGE);

        builder.Services.AddHttpClient<IBrasilApiProvider, BrasilApiProvider>(client =>
        {
            client.BaseAddress = new Uri("https://brasilapi.com.br");
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient<IIBGEProvider, IBGEProvider>(client =>
        {
            client.BaseAddress = new Uri("https://servicodados.ibge.gov.br");
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}
