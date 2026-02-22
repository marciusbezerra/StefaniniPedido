using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StefaniniPedido.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        // Usa SQL Server para migrations de produção
        if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "InMemory")
            optionsBuilder.UseInMemoryDatabase("StefaniniPedidoDB");
        else
            optionsBuilder.UseSqlServer(connectionString);


        return new AppDbContext(optionsBuilder.Options);
    }
}
