using DesafioIDez.Infraestrutura.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;

namespace DesafioIDez.Testes.Unitarios.Servicos;

public class RedisCacheServicoTests
{

    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private readonly RedisCacheServico _redisCacheServico;

    public RedisCacheServicoTests()
    {
        _distributedCacheMock = new Mock<IDistributedCache>();
        _redisCacheServico = new RedisCacheServico(_distributedCacheMock.Object);
    }

    [Fact]
    public async Task ObterAsync_Deve_RetornarObjetoQuandoExisteNoCache()
    {
        // Arrange
        var chave = "teste:objeto";
        var objeto = new { Nome = "Teste", Valor = 123 };
        var json = JsonSerializer.Serialize(objeto);
        var bytes = Encoding.UTF8.GetBytes(json);

        _distributedCacheMock
            .Setup(c => c.GetAsync(It.Is<string>(k => k == chave), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);

        // Act
        var resultado = await _redisCacheServico.ObterAsync<object>(chave);

        // Assert
        Assert.NotNull(resultado);
        var element = (JsonElement)resultado;
        Assert.Equal("Teste", element.GetProperty("Nome").GetString());
        Assert.Equal(123, element.GetProperty("Valor").GetInt32());
    }

    [Fact]
    public async Task ObterAsync_Deve_RetornarNullQuandoNaoExisteNoCache()
    {
        // Arrange
        var chave = "teste:nulo";

        _distributedCacheMock
            .Setup(c => c.GetAsync(It.Is<string>(k => k == chave), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        // Act
        var resultado = await _redisCacheServico.ObterAsync<object>(chave);

        // Assert
        Assert.Null(resultado);
    }

    [Fact]
    public async Task AdicionarAsync_Deve_ChamarSetAsyncComJsonCorreto()
    {
        // Arrange
        var chave = "teste:adicionar";
        var objeto = new { Nome = "Adicionar", Valor = 999 };
        byte[]? capturadoBytes = null;

        _distributedCacheMock
            .Setup(c => c.SetAsync(
                It.Is<string>(k => k == chave),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>((k, bytes, options, token) =>
            {
                capturadoBytes = bytes;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _redisCacheServico.AdicionarAsync(chave, objeto, TimeSpan.FromMinutes(5));

        // Assert
        Assert.NotNull(capturadoBytes);
        var json = System.Text.Encoding.UTF8.GetString(capturadoBytes!);
        Assert.Contains("Adicionar", json);
        Assert.Contains("999", json);
    }

}
