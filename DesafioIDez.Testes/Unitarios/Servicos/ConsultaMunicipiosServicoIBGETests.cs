using DesafioIDez.Aplicacao.DTO;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Aplicacao.Servicos;
using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;
using Moq;

namespace DesafioIDez.Testes.Unitarios.Servicos;

public class ConsultaMunicipiosServicoIBGETests
{
    private readonly Mock<IIBGEProvider> _ibgeProviderMock;
    private readonly Mock<IRedisCacheServico> _redisCacheServicoMock;
    private readonly ConsultaMunicipiosIBGE _consultaMunicipiosServico;

    public ConsultaMunicipiosServicoIBGETests()
    {
        _ibgeProviderMock = new Mock<IIBGEProvider>();
        _redisCacheServicoMock = new Mock<IRedisCacheServico>();
        _consultaMunicipiosServico = new ConsultaMunicipiosIBGE(
            _ibgeProviderMock.Object,
            _redisCacheServicoMock.Object);
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_PorEstado_Deve_RetornarListaPreenchida()
    {
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
        };

        _ibgeProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>
            {
                new Municipio { Name = "São Paulo", IBGE_Code = "3550308" },
                new Municipio { Name = "Campinas", IBGE_Code = "3509502" }
            });

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Itens);
        Assert.Equal(2, resultado.Itens.Count);
        Assert.Contains(resultado.Itens, m => m.Name == "São Paulo" && m.IBGE_Code == "3550308");
        Assert.Contains(resultado.Itens, m => m.Name == "Campinas" && m.IBGE_Code == "3509502");
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_PorEstado_E_PorMunicipio_Deve_RetornarListaPreenchida()
    {
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
            Municipio = "São Paulo"
        };

        _ibgeProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>
            {
                new Municipio { Name = "São Paulo", IBGE_Code = "3550308" },
            });

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Itens);
        Assert.Single(resultado.Itens);
        Assert.Contains(resultado.Itens, m => m.Name == "São Paulo" && m.IBGE_Code == "3550308");
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_PorEstado_E_PorCodigoIbge_Deve_RetornarListaPreenchida()
    {
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
            Codigo_Ibge = "3550308"
        };

        _ibgeProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>
            {
                new Municipio { Name = "São Paulo", IBGE_Code = "3550308" },
            });

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.NotNull(resultado.Itens);
        Assert.Single(resultado.Itens);
        Assert.Contains(resultado.Itens, m => m.Name == "São Paulo" && m.IBGE_Code == "3550308");
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_PorEstadoInvalido_Deve_RetornarListaVazia()
    {
        // Arrange
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "XX",
        };

        _ibgeProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>());

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado.Itens);
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_PorEstadoValido_E_MunicipioInvalido_Deve_RetornarListaVazia()
    {
        // Arrange
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
            Municipio = "zzzzzzzz"
        };

        _ibgeProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>());

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado.Itens);
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_PorEstadoValido_E_MunicipioValido_E_CodigoIbgeInvalido_Deve_RetornarListaVazia()
    {
        // Arrange
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
            Municipio = "São Paulo",
            Codigo_Ibge = "abc12312d"
        };

        _ibgeProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>());

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado.Itens);
    }
}
