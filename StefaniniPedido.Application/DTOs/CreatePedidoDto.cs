namespace StefaniniPedido.Application.DTOs;

public class CreatePedidoDto
{
    public string NomeCliente { get; set; } = string.Empty;
    public string EmailCliente { get; set; } = string.Empty;
    public List<CreateItensPedidoDto> ItensPedido { get; set; } = new();
}
