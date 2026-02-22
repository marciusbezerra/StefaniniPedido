using Microsoft.EntityFrameworkCore;
using StefaniniPedido.Domain.Entities;
using StefaniniPedido.Domain.Interfaces;
using StefaniniPedido.Infrastructure.Data;

namespace StefaniniPedido.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        return await _context.Pedidos
            .Include(p => p.ItensPedido)
                .ThenInclude(i => i.Produto)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Pedido?> ObterPorIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.ItensPedido)
                .ThenInclude(i => i.Produto)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pedido> CriarAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<Pedido> AtualizarAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task DeletarAsync(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido is not null)
        {
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Pedidos.AnyAsync(p => p.Id == id);
    }
}
