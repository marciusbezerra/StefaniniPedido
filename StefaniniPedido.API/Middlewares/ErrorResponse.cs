namespace StefaniniPedido.API.Middlewares;

public sealed class ErrorResponse
{
    public int StatusCode { get; init; }
    public string Error { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Detail { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
