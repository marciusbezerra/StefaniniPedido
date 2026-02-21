using StefaniniPedido.Domain.Entities;

namespace StefaniniPedido.Domain.Interfaces;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>> ObterTodosAsync();
    Task<Produto?> ObterPorIdAsync(int id);
    Task<Produto> CriarAsync(Produto produto);
    Task<Produto> AtualizarAsync(Produto produto);
    Task DeletarAsync(int id);
    Task<bool> ExisteAsync(int id);
}
