using Microsoft.AspNetCore.Mvc;
using StefaniniPedido.Application.DTOs;
using StefaniniPedido.Application.Interfaces;

namespace StefaniniPedido.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    /// <summary>Retorna todos os pedidos</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PedidoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await _pedidoService.ObterTodosAsync();
        return Ok(pedidos);
    }

    /// <summary>Retorna um pedido pelo ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var pedido = await _pedidoService.ObterPorIdAsync(id);
        if (pedido is null) return NotFound(new { message = $"Pedido {id} não encontrado." });
        return Ok(pedido);
    }

    /// <summary>Cria um novo pedido</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePedidoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var pedido = await _pedidoService.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
    }

    /// <summary>Atualiza um pedido existente</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePedidoDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var pedido = await _pedidoService.AtualizarAsync(id, dto);
        if (pedido is null) return NotFound(new { message = $"Pedido {id} não encontrado." });
        return Ok(pedido);
    }

    /// <summary>Remove um pedido</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var removido = await _pedidoService.DeletarAsync(id);
        if (!removido) return NotFound(new { message = $"Pedido {id} não encontrado." });
        return NoContent();
    }
}
