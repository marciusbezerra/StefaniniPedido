namespace StefaniniPedido.Application.DTOs;

public class UpdatePedidoDto
{
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public bool Pago { get; set; }
}
