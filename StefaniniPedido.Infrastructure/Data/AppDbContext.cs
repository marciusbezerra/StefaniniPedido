using Microsoft.EntityFrameworkCore;
using StefaniniPedido.Domain.Entities;

namespace StefaniniPedido.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItensPedido> ItensPedido => Set<ItensPedido>();
    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeCliente).HasMaxLength(60).IsRequired();
            entity.Property(e => e.EmailCliente).HasMaxLength(60).IsRequired();
            entity.Property(e => e.DataCriacao).IsRequired();
            entity.Property(e => e.Pago).IsRequired();
            entity.Ignore(e => e.ValorTotal);
            entity.HasMany(e => e.ItensPedido)
                  .WithOne(i => i.Pedido)
                  .HasForeignKey(i => i.IdPedido)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItensPedido>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantidade).IsRequired();
            entity.HasOne(e => e.Produto)
                  .WithMany()
                  .HasForeignKey(e => e.IdProduto)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeProduto).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Valor).HasColumnType("decimal(10,2)").IsRequired();
        });

        // Seed data
        modelBuilder.Entity<Produto>().HasData(
            new { Id = 1, NomeProduto = "Notebook", Valor = 3500.00m },
            new { Id = 2, NomeProduto = "Mouse", Valor = 89.90m },
            new { Id = 3, NomeProduto = "Teclado", Valor = 149.90m },
            new { Id = 4, NomeProduto = "Monitor", Valor = 1299.00m }
        );
    }
}
