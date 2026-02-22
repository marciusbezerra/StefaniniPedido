using StefaniniPedido.Application.DTOs;
using StefaniniPedido.Application.Interfaces;
using StefaniniPedido.Domain.Entities;
using StefaniniPedido.Domain.Interfaces;

namespace StefaniniPedido.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository, 
    IProdutoRepository produtoRepository) : IPedidoService
{
    public async Task<IEnumerable<PedidoDto>> ObterTodosAsync()
    {
        var pedidos = await pedidoRepository.ObterTodosAsync();
        return pedidos.Select(MapToDto);
    }

    public async Task<PedidoDto?> ObterPorIdAsync(int id)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id);
        return pedido is null ? null : MapToDto(pedido);
    }

    public async Task<PedidoDto> CriarAsync(CreatePedidoDto dto)
    {
        var pedido = new Pedido(dto.NomeCliente, dto.EmailCliente);
        var pedidoCriado = await pedidoRepository.CriarAsync(pedido);

        foreach (var itemDto in dto.ItensPedido)
        {
            var item = new ItensPedido(pedidoCriado.Id, itemDto.IdProduto, itemDto.Quantidade);
            pedidoCriado.AdicionarItem(item);
        }

        var pedidoAtualizado = await pedidoRepository.AtualizarAsync(pedidoCriado);
        return MapToDto(pedidoAtualizado);
    }

    public async Task<PedidoDto?> AtualizarAsync(int id, UpdatePedidoDto dto)
    {
        var pedido = await pedidoRepository.ObterPorIdAsync(id);
        if (pedido is null) return null;

        pedido.Atualizar(dto.NomeCliente, dto.EmailCliente, dto.Pago);
        var atualizado = await pedidoRepository.AtualizarAsync(pedido);
        return MapToDto(atualizado);
    }

    public async Task<bool> DeletarAsync(int id)
    {
        if (!await pedidoRepository.ExisteAsync(id)) return false;
        await pedidoRepository.DeletarAsync(id);
        return true;
    }

    private static PedidoDto MapToDto(Pedido pedido) => new()
    {
        Id = pedido.Id,
        NomeCliente = pedido.NomeCliente,
        EmailCliente = pedido.EmailCliente,
        Pago = pedido.Pago,
        ValorTotal = pedido.ValorTotal,
        ItensPedido = pedido.ItensPedido.Select(i => new ItensPedidoDto
        {
            Id = i.Id,
            IdProduto = i.IdProduto,
            NomeProduto = i.Produto?.NomeProduto ?? string.Empty,
            ValorUnitario = i.Produto?.Valor ?? 0,
            Quantidade = i.Quantidade
        }).ToList()
    };
}
