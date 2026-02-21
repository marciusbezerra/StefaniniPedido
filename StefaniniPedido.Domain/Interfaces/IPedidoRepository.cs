using StefaniniPedido.Domain.Entities;

namespace StefaniniPedido.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> ObterTodosAsync();
    Task<Pedido?> ObterPorIdAsync(int id);
    Task<Pedido> CriarAsync(Pedido pedido);
    Task<Pedido> AtualizarAsync(Pedido pedido);
    Task DeletarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
