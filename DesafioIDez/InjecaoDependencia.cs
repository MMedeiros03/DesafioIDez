using DesafioIDez.Aplicacao.Enums;
using DesafioIDez.Aplicacao.Factories;
using DesafioIDez.Aplicacao.Interfaces.Factory;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Aplicacao.Servicos;
using DesafioIDez.Dominio.Interfaces.Providers;
using DesafioIDez.Infraestrutura.Cache;
using DesafioIDez.Infraestrutura.Providers;

namespace DesafioIDez.Api;

public static class InjecaoDependencia
{
    public static void AdicionarDependencias(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IConsultaMunicipiosFactory, ConsultaMunicipiosFactory>();
        builder.Services.AddScoped<IBrasilApiProvider, BrasilApiProvider>();
        builder.Services.AddScoped<IIBGEProvider, IBGEProvider>();
        builder.Services.AddScoped<IRedisCacheServico, RedisCacheServico>();

        builder.Services.AddKeyedScoped<IConsultaMunicipiosServico, ConsultaMunicipiosBrasilApi>(ProviderEnum.BrasilApi);
        builder.Services.AddKeyedScoped<IConsultaMunicipiosServico, ConsultaMunicipiosIBGE>(ProviderEnum.IBGE);

        builder.Services.AddHttpClient<IBrasilApiProvider, BrasilApiProvider>((sp, client) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var baseUrl = configuration["BrasilApi:BaseUrl"]
                ?? throw new InvalidOperationException("BrasilApi:BaseUrl não configurado.");

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpClient<IIBGEProvider, IBGEProvider>((sp, client) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var baseUrl = configuration["IBGEApi:BaseUrl"]
                ?? throw new InvalidOperationException("IBGEApi:BaseUrl não configurado.");

            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}
