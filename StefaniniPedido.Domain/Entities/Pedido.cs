namespace StefaniniPedido.Domain.Entities;

public class Pedido
{
    public int Id { get; private set; }
    public string NomeCliente { get; private set; } = string.Empty;
    public string EmailCliente { get; private set; } = string.Empty;
    public DateTime DataCriacao { get; private set; }
    public bool Pago { get; private set; }

    private readonly List<ItensPedido> _itensPedido = new();
    public IReadOnlyCollection<ItensPedido> ItensPedido => _itensPedido.AsReadOnly();

    protected Pedido() { }

    public Pedido(string nomeCliente, string emailCliente)
    {
        NomeCliente = nomeCliente;
        EmailCliente = emailCliente;
        DataCriacao = DateTime.UtcNow;
        Pago = false;
    }

    public void Atualizar(string nomeCliente, string emailCliente, bool pago)
    {
        NomeCliente = nomeCliente;
        EmailCliente = emailCliente;
        Pago = pago;
    }

    public void MarcarComoPago() => Pago = true;

    public void AdicionarItem(ItensPedido item) => _itensPedido.Add(item);

    public decimal ValorTotal => _itensPedido.Sum(i => i.Produto != null
        ? i.Produto.Valor * i.Quantidade
        : 0);
}
