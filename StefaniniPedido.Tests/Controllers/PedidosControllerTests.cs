using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using StefaniniPedido.API.Controllers;
using StefaniniPedido.Application.DTOs;
using StefaniniPedido.Application.Interfaces;

namespace StefaniniPedido.Tests.Controllers;

public class PedidosControllerTests
{
    private readonly Mock<IPedidoService> _serviceMock;
    private readonly PedidosController _controller;

    public PedidosControllerTests()
    {
        _serviceMock = new Mock<IPedidoService>();
        _controller = new PedidosController(_serviceMock.Object);
    }

    [Fact(DisplayName = "GET /api/pedidos - Deve retornar 200 OK com lista de pedidos")]
    public async Task GetAll_DeveRetornar200OkComListaDePedidos()
    {
        // Arrange
        var pedidos = new List<PedidoDto>
        {
            new() { Id = 1, NomeCliente = "João Silva", EmailCliente = "joao@email.com", Pago = false, ValorTotal = 3500.00m },
            new() { Id = 2, NomeCliente = "Maria Souza", EmailCliente = "maria@email.com", Pago = true, ValorTotal = 89.90m }
        };
        _serviceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(pedidos);

        // Act
        var resultado = await _controller.GetAll();

        // Assert
        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var retorno = okResult.Value.Should().BeAssignableTo<IEnumerable<PedidoDto>>().Subject;
        retorno.Should().HaveCount(2);
    }

    [Fact(DisplayName = "GET /api/pedidos - Deve retornar 200 OK com lista vazia")]
    public async Task GetAll_DeveRetornar200OkComListaVazia()
    {
        // Arrange
        _serviceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<PedidoDto>());

        // Act
        var resultado = await _controller.GetAll();

        // Assert
        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var retorno = okResult.Value.Should().BeAssignableTo<IEnumerable<PedidoDto>>().Subject;
        retorno.Should().BeEmpty();
    }

    [Fact(DisplayName = "GET /api/pedidos/{id} - Deve retornar 200 OK com pedido existente")]
    public async Task GetById_DeveRetornar200OkComPedidoExistente()
    {
        // Arrange
        var pedido = new PedidoDto
        {
            Id = 1,
            NomeCliente = "João Silva",
            EmailCliente = "joao@email.com",
            Pago = false,
            ValorTotal = 3500.00m,
            ItensPedido = new List<ItensPedidoDto>
            {
                new() { Id = 1, IdProduto = 1, NomeProduto = "Notebook", ValorUnitario = 3500.00m, Quantidade = 1 }
            }
        };
        _serviceMock.Setup(s => s.ObterPorIdAsync(1)).ReturnsAsync(pedido);

        // Act
        var resultado = await _controller.GetById(1);

        // Assert
        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        var retorno = okResult.Value.Should().BeOfType<PedidoDto>().Subject;
        retorno.Id.Should().Be(1);
        retorno.NomeCliente.Should().Be("João Silva");
        retorno.ItensPedido.Should().HaveCount(1);
    }

    [Fact(DisplayName = "GET /api/pedidos/{id} - Deve retornar 404 quando pedido não existe")]
    public async Task GetById_DeveRetornar404QuandoPedidoNaoExiste()
    {
        // Arrange
        _serviceMock.Setup(s => s.ObterPorIdAsync(999)).ReturnsAsync((PedidoDto?)null);

        // Act
        var resultado = await _controller.GetById(999);

        // Assert
        var notFound = resultado.Should().BeOfType<NotFoundObjectResult>().Subject;
        notFound.StatusCode.Should().Be(404);
    }

    [Fact(DisplayName = "GET /api/pedidos - Deve retornar JSON no formato correto conforme o desafio")]
    public async Task GetAll_DeveRetornarJsonNoFormatoCorreto()
    {
        // Arrange – JSON exigido pelo desafio
        var pedido = new PedidoDto
        {
            Id = 1,
            NomeCliente = "Stefanini Teste",
            EmailCliente = "teste@stefanini.com",
            Pago = true,
            ValorTotal = 7089.90m,
            ItensPedido = new List<ItensPedidoDto>
            {
                new() { Id = 1, IdProduto = 1, NomeProduto = "Notebook", ValorUnitario = 3500.00m, Quantidade = 2 },
                new() { Id = 2, IdProduto = 2, NomeProduto = "Mouse", ValorUnitario = 89.90m, Quantidade = 1 }
            }
        };
        _serviceMock.Setup(s => s.ObterTodosAsync()).ReturnsAsync(new List<PedidoDto> { pedido });

        // Act
        var resultado = await _controller.GetAll();

        // Assert – valida o contrato JSON do desafio
        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        var lista = okResult.Value.Should().BeAssignableTo<IEnumerable<PedidoDto>>().Subject.ToList();
        lista.Should().HaveCount(1);

        var p = lista[0];
        p.Id.Should().BeGreaterThan(0);
        p.NomeCliente.Should().NotBeNullOrEmpty();
        p.EmailCliente.Should().NotBeNullOrEmpty();
        p.ValorTotal.Should().BePositive();
        p.ItensPedido.Should().NotBeEmpty();

        var item = p.ItensPedido[0];
        item.IdProduto.Should().BeGreaterThan(0);
        item.NomeProduto.Should().NotBeNullOrEmpty();
        item.ValorUnitario.Should().BePositive();
        item.Quantidade.Should().BeGreaterThan(0);
    }

    [Fact(DisplayName = "POST /api/pedidos - Deve retornar 201 Created com pedido criado")]
    public async Task Create_DeveRetornar201ComPedidoCriado()
    {
        // Arrange
        var dto = new CreatePedidoDto
        {
            NomeCliente = "Carlos Lima",
            EmailCliente = "carlos@email.com",
            ItensPedido = new List<CreateItensPedidoDto> { new() { IdProduto = 1, Quantidade = 1 } }
        };
        var retornoDto = new PedidoDto { Id = 1, NomeCliente = "Carlos Lima", EmailCliente = "carlos@email.com" };
        _serviceMock.Setup(s => s.CriarAsync(dto)).ReturnsAsync(retornoDto);

        // Act
        var resultado = await _controller.Create(dto);

        // Assert
        var created = resultado.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.StatusCode.Should().Be(201);
    }

    [Fact(DisplayName = "DELETE /api/pedidos/{id} - Deve retornar 204 NoContent ao deletar")]
    public async Task Delete_DeveRetornar204NoContent()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeletarAsync(1)).ReturnsAsync(true);

        // Act
        var resultado = await _controller.Delete(1);

        // Assert
        resultado.Should().BeOfType<NoContentResult>()
            .Which.StatusCode.Should().Be(204);
    }

    [Fact(DisplayName = "DELETE /api/pedidos/{id} - Deve retornar 404 ao deletar inexistente")]
    public async Task Delete_DeveRetornar404QuandoNaoExiste()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeletarAsync(999)).ReturnsAsync(false);

        // Act
        var resultado = await _controller.Delete(999);

        // Assert
        resultado.Should().BeOfType<NotFoundObjectResult>()
            .Which.StatusCode.Should().Be(404);
    }
}
