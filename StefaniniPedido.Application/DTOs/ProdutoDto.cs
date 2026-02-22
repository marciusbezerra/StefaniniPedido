namespace StefaniniPedido.Application.DTOs;

public class ProdutoDto
{
    public int Id { get; set; }
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}

public class CreateProdutoDto
{
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}

public class UpdateProdutoDto
{
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
