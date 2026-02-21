namespace StefaniniPedido.Domain.Entities;

public class Produto
{
    public int Id { get; private set; }
    public string NomeProduto { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }

    protected Produto() { }

    public Produto(string nomeProduto, decimal valor)
    {
        NomeProduto = nomeProduto;
        Valor = valor;
    }

    public void Atualizar(string nomeProduto, decimal valor)
    {
        NomeProduto = nomeProduto;
        Valor = valor;
    }
}
