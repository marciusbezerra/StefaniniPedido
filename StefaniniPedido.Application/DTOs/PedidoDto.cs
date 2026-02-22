namespace StefaniniPedido.Application.DTOs;

public class PedidoDto
{
    public int Id { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public bool Pago { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItensPedidoDto> ItensPedido { get; set; } = new();
}
