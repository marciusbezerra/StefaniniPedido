using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StefaniniPedido.Application.Interfaces;
using StefaniniPedido.Application.Services;
using StefaniniPedido.Domain.Interfaces;
using StefaniniPedido.Infrastructure.Data;
using StefaniniPedido.Infrastructure.Repositories;

namespace StefaniniPedido.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // Se não estiver em Container, usar InMemory como pedido no Desafio

        if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "InMemory")
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("StefaniniPedidoDb"));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IProdutoService, ProdutoService>();

        return services;
    }
}
