using Microsoft.AspNetCore.Mvc;
using StefaniniPedido.Application.DTOs;
using StefaniniPedido.Application.Interfaces;

namespace StefaniniPedido.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>Retorna todos os produtos</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var produtos = await _produtoService.ObterTodosAsync();
        return Ok(produtos);
    }

    /// <summary>Retorna um produto pelo ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var produto = await _produtoService.ObterPorIdAsync(id);
        if (produto is null) return NotFound(new { message = $"Produto {id} não encontrado." });
        return Ok(produto);
    }

    /// <summary>Cria um novo produto</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProdutoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var produto = await _produtoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    /// <summary>Atualiza um produto existente</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProdutoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var produto = await _produtoService.AtualizarAsync(id, dto);
        if (produto is null) return NotFound(new { message = $"Produto {id} não encontrado." });
        return Ok(produto);
    }

    /// <summary>Remove um produto</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _produtoService.DeletarAsync(id);
        if (!removido) return NotFound(new { message = $"Produto {id} não encontrado." });
        return NoContent();
    }
}
