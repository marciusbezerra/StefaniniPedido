using StefaniniPedido.Application.DTOs;
using StefaniniPedido.Application.Interfaces;
using StefaniniPedido.Domain.Entities;
using StefaniniPedido.Domain.Interfaces;

namespace StefaniniPedido.Application.Services;

public class ProdutoService(IProdutoRepository produtoRepository) : IProdutoService
{
    public async Task<IEnumerable<ProdutoDto>> ObterTodosAsync()
    {
        var produtos = await produtoRepository.ObterTodosAsync();
        return produtos.Select(MapToDto);
    }

    public async Task<ProdutoDto?> ObterPorIdAsync(int id)
    {
        var produto = await produtoRepository.ObterPorIdAsync(id);
        return produto is null ? null : MapToDto(produto);
    }

    public async Task<ProdutoDto> CriarAsync(CreateProdutoDto dto)
    {
        var produto = new Produto(dto.NomeProduto, dto.Valor);
        var criado = await produtoRepository.CriarAsync(produto);
        return MapToDto(criado);
    }

    public async Task<ProdutoDto?> AtualizarAsync(int id, UpdateProdutoDto dto)
    {
        var produto = await produtoRepository.ObterPorIdAsync(id);
        if (produto is null) return null;

        produto.Atualizar(dto.NomeProduto, dto.Valor);
        var atualizado = await produtoRepository.AtualizarAsync(produto);
        return MapToDto(atualizado);
    }

    public async Task<bool> DeletarAsync(int id)
    {
        if (!await produtoRepository.ExisteAsync(id)) return false;
        await produtoRepository.DeletarAsync(id);
        return true;
    }

    private static ProdutoDto MapToDto(Produto produto) => new()
    {
        Id = produto.Id,
        NomeProduto = produto.NomeProduto,
        Valor = produto.Valor
    };
}
