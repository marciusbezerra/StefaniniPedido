namespace StefaniniPedido.Domain.Entities;

public class ItensPedido
{
    public int Id { get; private set; }
    public int IdPedido { get; private set; }
    public int IdProduto { get; private set; }
    public int Quantidade { get; private set; }

    public Pedido? Pedido { get; private set; }
    public Produto? Produto { get; private set; }

    protected ItensPedido() { }

    public ItensPedido(int idPedido, int idProduto, int quantidade)
    {
        IdPedido = idPedido;
        IdProduto = idProduto;
        Quantidade = quantidade;
    }

    public void Atualizar(int idProduto, int quantidade)
    {
        IdProduto = idProduto;
        Quantidade = quantidade;
    }
}
