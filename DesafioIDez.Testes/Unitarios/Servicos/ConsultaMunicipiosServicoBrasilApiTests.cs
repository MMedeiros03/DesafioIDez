using DesafioIDez.Aplicacao.DTO;
using DesafioIDez.Aplicacao.Interfaces.Servicos;
using DesafioIDez.Aplicacao.Servicos;
using DesafioIDez.Dominio.Entidades;
using DesafioIDez.Dominio.Interfaces.Providers;
using Moq;

namespace DesafioIDez.Testes.Unitarios.Servicos;

public class ConsultaMunicipiosServicoBrasilApiTests
{
    private readonly Mock<IBrasilApiProvider> _brasilApiProviderMock;
    private readonly Mock<IRedisCacheServico> _redisCacheServicoMock;
    private readonly ConsultaMunicipiosBrasilApi _consultaMunicipiosServico;

    public ConsultaMunicipiosServicoBrasilApiTests()
    {
        _brasilApiProviderMock = new Mock<IBrasilApiProvider>();
        _redisCacheServicoMock = new Mock<IRedisCacheServico>();
        _consultaMunicipiosServico = new ConsultaMunicipiosBrasilApi(
            _brasilApiProviderMock.Object,
            _redisCacheServicoMock.Object);
    }

    [Fact]
    public async Task ConsultarMunicipiosPorEstadoAsync_Deve_RetornarListaPreenchida()
    {
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
        };

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
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
    public async Task ConsultarMunicipiosPorEstadoAsync_E_PorMunicipio_Deve_RetornarListaPreenchida()
    {
        var filtro = new FiltroEstadoDto
        {
            Pagina = 1,
            TamanhoPagina = 10,
            Estado = "SP",
            Municipio = "São Paulo"
        };

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
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

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
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
    public async Task ConsultarMunicipiosPorEstadoAsync_Deve_RetornarSegundaPaginaCorreta()
    {
        var filtro = new FiltroEstadoDto
        {
            Pagina = 2,
            TamanhoPagina = 2,
            Estado = "SP"
        };

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>
            {
                new Municipio { Name = "São Paulo", IBGE_Code = "3550308" },
                new Municipio { Name = "Campinas", IBGE_Code = "3509502" },
                new Municipio { Name = "Santos", IBGE_Code = "3548500" },
                new Municipio { Name = "Sorocaba", IBGE_Code = "3552205" }
            });

        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        Assert.Equal(2, resultado.Itens.Count);
        Assert.Contains(resultado.Itens, m => m.Name == "Santos");
        Assert.Contains(resultado.Itens, m => m.Name == "Sorocaba");
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

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
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

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
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

        _brasilApiProviderMock.Setup(p => p.ObterMunicipiosPorEstadoAsync(filtro.Estado))
            .ReturnsAsync(new List<Municipio>());

        // Act
        var resultado = await _consultaMunicipiosServico.ConsultarMunicipiosPorEstadoAsync(filtro);

        // Assert
        Assert.NotNull(resultado);
        Assert.Empty(resultado.Itens);
    }
}
