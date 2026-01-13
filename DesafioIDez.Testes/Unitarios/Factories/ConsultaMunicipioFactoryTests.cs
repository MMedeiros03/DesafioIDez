using DesafioIDez.Aplicacao.Enums;
using DesafioIDez.Aplicacao.Factories;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DesafioIDez.Testes.Unitarios.Factories;

public class ConsultaMunicipioFactoryTests
{
    private readonly Mock<IKeyedServiceProvider> _serviceProviderMock;
    private readonly ConsultaMunicipiosFactory _factory;

    public ConsultaMunicipioFactoryTests()
    {
        _serviceProviderMock = new Mock<IKeyedServiceProvider>();
        _factory = new ConsultaMunicipiosFactory(_serviceProviderMock.Object);
    }

    [Fact]
    public void ObterConsulta_DeveRetornarServico_QuandoProviderForValidoERegistrado_BrasilApi()
    {
        var servicoMock = new Mock<IConsultaMunicipiosServico>();

        _serviceProviderMock
            .Setup(s => s.GetKeyedService(typeof(IConsultaMunicipiosServico), ProviderEnum.BrasilApi))
            .Returns(servicoMock.Object);

        var resultado = _factory.ObterConsulta("BrasilApi");

        Assert.Equal(servicoMock.Object, resultado);
    }

    [Fact]
    public void ObterConsulta_DeveRetornarServico_QuandoProviderForValidoERegistrado_IBGE()
    {
        var servicoMock = new Mock<IConsultaMunicipiosServico>();

        _serviceProviderMock
            .Setup(s => s.GetKeyedService(typeof(IConsultaMunicipiosServico), ProviderEnum.IBGE))
            .Returns(servicoMock.Object);

        var resultado = _factory.ObterConsulta("IBGE");

        Assert.Equal(servicoMock.Object, resultado);
    }

    [Fact]
    public void ObterConsulta_DeveLancarInvalidOperationException_QuandoEnumInvalido()
    {
        // Arrange
        string providerInvalido = "Inexistente";

        // Act & Assert
        var excecao = Assert.Throws<InvalidOperationException>(() =>
            _factory.ObterConsulta(providerInvalido));

        Assert.Equal("Provider inválido ou não configurado.", excecao.Message);
    }

    [Fact]
    public void ObterConsulta_DeveLancarNotImplementedException_QuandoNaoEncontrarNoContainer()
    {
        // Arrange
        string providerString = "IBGE";

        _serviceProviderMock
            .As<IKeyedServiceProvider>()
            .Setup(s => s.GetKeyedService(typeof(IConsultaMunicipiosServico), ProviderEnum.IBGE))
            .Returns(null);

        // Act
        var excecao = Assert.Throws<NotImplementedException>(() =>
            _factory.ObterConsulta(providerString));

        // Assert
        Assert.Contains(providerString, excecao.Message);
    }
}
