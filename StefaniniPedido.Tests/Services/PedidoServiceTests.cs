using Moq;
using FluentAssertions;
using StefaniniPedido.Application.DTOs;
using StefaniniPedido.Application.Services;
using StefaniniPedido.Domain.Entities;
using StefaniniPedido.Domain.Interfaces;

namespace StefaniniPedido.Tests.Services;

public class PedidoServiceTests
{
    private readonly Mock<IPedidoRepository> _pedidoRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly PedidoService _service;

    public PedidoServiceTests()
    {
        _pedidoRepoMock = new Mock<IPedidoRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new PedidoService(_pedidoRepoMock.Object, _unitOfWorkMock.Object);
    }

    [Fact(DisplayName = "ObterTodos - Deve retornar lista de pedidos")]
    public async Task ObterTodosAsync_DeveRetornarListaDePedidos()
    {
        // Arrange
        var pedidos = new List<Pedido>
        {
            CriarPedidoComItens(1, "João Silva", "joao@email.com"),
            CriarPedidoComItens(2, "Maria Souza", "maria@email.com")
        };
        _pedidoRepoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(pedidos);

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        _pedidoRepoMock.Verify(r => r.ObterTodosAsync(), Times.Once);
    }

    [Fact(DisplayName = "ObterTodos - Deve retornar lista vazia quando não há pedidos")]
    public async Task ObterTodosAsync_DeveRetornarListaVaziaQuandoNaoHaPedidos()
    {
        // Arrange
        _pedidoRepoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(new List<Pedido>());

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        resultado.Should().BeEmpty();
    }

    [Fact(DisplayName = "ObterPorId - Deve retornar pedido existente")]
    public async Task ObterPorIdAsync_DeveRetornarPedidoExistente()
    {
        // Arrange
        var pedido = CriarPedidoComItens(1, "João Silva", "joao@email.com");
        _pedidoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(pedido);

        // Act
        var resultado = await _service.ObterPorIdAsync(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.NomeCliente.Should().Be("João Silva");
        resultado.EmailCliente.Should().Be("joao@email.com");
        _pedidoRepoMock.Verify(r => r.ObterPorIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "ObterPorId - Deve retornar null quando pedido não existe")]
    public async Task ObterPorIdAsync_DeveRetornarNullQuandoPedidoNaoExiste()
    {
        // Arrange
        _pedidoRepoMock.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((Pedido?)null);

        // Act
        var resultado = await _service.ObterPorIdAsync(999);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact(DisplayName = "ObterPorId - Deve mapear corretamente o DTO com itens e valor total")]
    public async Task ObterPorIdAsync_DeveMapearCorretamenteDtoComItensEValorTotal()
    {
        // Arrange
        var pedido = CriarPedidoComItens(1, "João Silva", "joao@email.com");
        _pedidoRepoMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(pedido);

        // Act
        var resultado = await _service.ObterPorIdAsync(1);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.ItensPedido.Should().NotBeEmpty();
        resultado.ValorTotal.Should().BeGreaterThan(0);
        resultado.ItensPedido.First().NomeProduto.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "ObterTodos - Deve mapear corretamente a propriedade Pago")]
    public async Task ObterTodosAsync_DeveMapearCorretamentePago()
    {
        // Arrange
        var pedidoPago = CriarPedidoComItens(1, "Ana Costa", "ana@email.com");
        pedidoPago.MarcarComoPago();
        _pedidoRepoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(new List<Pedido> { pedidoPago });

        // Act
        var resultado = await _service.ObterTodosAsync();

        // Assert
        resultado.First().Pago.Should().BeTrue();
    }

    [Fact(DisplayName = "Criar - Deve criar pedido e retornar DTO")]
    public async Task CriarAsync_DeveCriarPedidoERetornarDto()
    {
        // Arrange
        var dto = new CreatePedidoDto
        {
            NomeCliente = "Carlos Lima",
            EmailCliente = "carlos@email.com",
            ItensPedido = new List<CreateItensPedidoDto>
            {
                new() { IdProduto = 1, Quantidade = 2 }
            }
        };
        var pedidoCriado = new Pedido("Carlos Lima", "carlos@email.com");
        _pedidoRepoMock.Setup(r => r.CriarAsync(It.IsAny<Pedido>())).ReturnsAsync(pedidoCriado);
        _pedidoRepoMock.Setup(r => r.AtualizarAsync(It.IsAny<Pedido>())).ReturnsAsync(pedidoCriado);

        // Act
        var resultado = await _service.CriarAsync(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.NomeCliente.Should().Be("Carlos Lima");
        _pedidoRepoMock.Verify(r => r.CriarAsync(It.IsAny<Pedido>()), Times.Once);
    }

    [Fact(DisplayName = "Deletar - Deve retornar false quando pedido não existe")]
    public async Task DeletarAsync_DeveRetornarFalseQuandoPedidoNaoExiste()
    {
        // Arrange
        _pedidoRepoMock.Setup(r => r.ExisteAsync(999)).ReturnsAsync(false);

        // Act
        var resultado = await _service.DeletarAsync(999);

        // Assert
        resultado.Should().BeFalse();
        _pedidoRepoMock.Verify(r => r.DeletarAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact(DisplayName = "Deletar - Deve retornar true quando pedido existe")]
    public async Task DeletarAsync_DeveRetornarTrueQuandoPedidoExiste()
    {
        // Arrange
        _pedidoRepoMock.Setup(r => r.ExisteAsync(1)).ReturnsAsync(true);
        _pedidoRepoMock.Setup(r => r.DeletarAsync(1)).Returns(Task.CompletedTask);

        // Act
        var resultado = await _service.DeletarAsync(1);

        // Assert
        resultado.Should().BeTrue();
        _pedidoRepoMock.Verify(r => r.DeletarAsync(1), Times.Once);
    }

    // Helper para criar pedido com itens via reflexão
    private static Pedido CriarPedidoComItens(int id, string nome, string email)
    {
        var pedido = new Pedido(nome, email);
        SetPrivateId(pedido, id);

        var produto = new Produto("Notebook", 3500.00m);
        SetPrivateId(produto, 1);

        var item = new ItensPedido(id, 1, 2);
        SetProduto(item, produto);
        pedido.AdicionarItem(item);

        return pedido;
    }

    private static void SetPrivateId<T>(T entity, int id) where T : class
    {
        typeof(T).GetProperty("Id")!
            .SetValue(entity, id, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, null, null);
    }

    private static void SetProduto(ItensPedido item, Produto produto)
    {
        typeof(ItensPedido).GetProperty("Produto")!
            .SetValue(item, produto, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, null, null);
    }
}
