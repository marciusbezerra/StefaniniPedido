using StefaniniPedido.Application.DTOs;

namespace StefaniniPedido.Application.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoDto>> ObterTodosAsync();
    Task<PedidoDto?> ObterPorIdAsync(int id);
    Task<PedidoDto> CriarAsync(CreatePedidoDto dto);
    Task<PedidoDto?> AtualizarAsync(int id, UpdatePedidoDto dto);
    Task<bool> DeletarAsync(int id);
}
