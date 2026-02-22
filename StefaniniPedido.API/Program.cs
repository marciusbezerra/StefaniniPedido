using Microsoft.EntityFrameworkCore;
using StefaniniPedido.API.Middlewares;
using StefaniniPedido.Infrastructure.Data;
using StefaniniPedido.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Stefanini Pedidos API",
        Version = "v1",
        Description = "API de CRUD de Pedidos - Desafio Stefanini",
        Contact = new() { Name = "Stefanini", Email = "contato@stefanini.com" }
    });
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

builder.Services.AddInfrastructure(builder.Configuration);

// CORS para os clientes React/Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// OBS: Apenas para facilitar o Desafio, auto migration do banco e seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var stringConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(stringConnection) || stringConnection == "InMemory")
        db.Database.EnsureCreated();
    else
        db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stefanini Pedidos API v1");
    c.RoutePrefix = string.Empty; // Importante! Como é um Desafio, deixei a UI do Swagger na raiz.
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
