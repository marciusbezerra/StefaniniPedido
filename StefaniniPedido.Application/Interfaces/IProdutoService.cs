using StefaniniPedido.Application.DTOs;

namespace StefaniniPedido.Application.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoDto>> ObterTodosAsync();
    Task<ProdutoDto?> ObterPorIdAsync(int id);
    Task<ProdutoDto> CriarAsync(CreateProdutoDto dto);
    Task<ProdutoDto?> AtualizarAsync(int id, UpdateProdutoDto dto);
    Task<bool> DeletarAsync(int id);
}
