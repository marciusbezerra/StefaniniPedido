# 🛒 Stefanini Pedidos API

Sistema completo de CRUD de Pedidos desenvolvido como desafio técnico Stefanini, utilizando **.NET 10**, **Clean Architecture**, **DDD**, **SOLID** e **Clean Code**, com dois clientes front-end: **React** e **Angular**.

---

## Executando no Docker

Crie um arquivo .env na mesma pasta do docker-compose.yml (/), contendo uma senha _strongly SQL Server like_, ex:

```ini
SA_PASSWORD=StefaniniPass@123
```

OBS. Arquivo .env minimalista, não submetido apenas para seguir boas práticas 

Executar os seguintes comandos

```bash
# Clonar e subir tudo
git clone <repo-url>
cd StefaniniPedido
# .env copiar o .env agora para cá
docker compose up -d --build
```

| Serviço            | URL                   |
| ------------------ | --------------------- |
| **Swagger (API)**  | http://localhost:5000 |
| **React Client**   | http://localhost:3002 |
| **Angular Client** | http://localhost:4200 |
| **SQL Server**     | localhost:1433        |

**IMPORTANTE: SEM .ENV VAI RUIM!! 😭**

---

## Aplicação Online (Server Próprio)

| Serviço            | URL                                  |
| ------------------ | ------------------------------------ |
| **Swagger (API)**  | https://stefanini.marciusbezerra.com |
| **React Client**   | https://react.marciusbezerra.com     |
| **Angular Client** | https://angular.marciusbezerra.com   |

---

## OBSERVAÇÕES IMPORTANTES

- Autenticação / Token JWT não feito por não constar no Desafio
- Como a aplicação está pública, *.marciusbezerra.com, pode conter dados ofensivos / imprevisíveis (de curiosos)
- Importante! Como é um Desafio, deixei a UI do Swagger na raiz
- A API foi extensamente documenta, é comum fazer um código mais limpo
- Como se trata de um Desafio, o CORS foi completamente LIBERADO
- Apenas para facilitar o Desafio, fiz auto migration e seed do banco
- Para `dotnet run`, o padrão é InMemory, salvo se os appSettings forem modificados
- Para docker, o padrão é volume persistente
- UoW usado apenas para transaçães, já que o próprio EF DbContext já é um UoW

---

## Arquitetura

```
StefaniniPedido/
├── StefaniniPedido.Domain/          # Entidades, Interfaces (sem dependências externas)
├── StefaniniPedido.Application/     # DTOs, Services, Interfaces de serviços
├── StefaniniPedido.Infrastructure/  # EF Core, Repositories, DependencyInjection
├── StefaniniPedido.API/             # Controllers, Program.cs, Swagger
├── StefaniniPedido.Tests/           # Testes unitários (xUnit + Moq + FluentAssertions)
├── clients/
│   ├── pedido-angular/                  # Front-end Angular 19 + Bootstrap 5
│   └── pedido-react/                    # Front-end React 18 + Vite + Bootstrap 5
├── Dockerfile                           # Build da API
├── docker-compose.yml                   # Orquestração completa
└── README.md
```

**Camadas (Clean Architecture):**
- **Domain**: Entidades `Pedido`, `ItensPedido`, `Produto` com regras de negócio e interfaces de repositório
- **Application**: Serviços com mapeamento DTO, lógica de negócio desacoplada
- **Infrastructure**: EF Core (SQL Server / InMemory), Repositórios, Migrations, DI
- **API**: Controllers REST, Swagger, CORS

---

## Execução Local (sem Docker)

OBS. banco InMemory (não necessita SQL Server)

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 20+](https://nodejs.org)
- [Angular CLI 19+](https://angular.io/cli): `npm install -g @angular/cli`

### API (.NET)

```bash
cd StefaniniPedido.API

# dotnet ef migrations add InitialCreate \
#  --project StefaniniPedido.Infrastructure \
#  --startup-project StefaniniPedido.API \
#  --output-dir Data/Migrations

dotnet ef database update \
  --project StefaniniPedido.Infrastructure \
  --startup-project StefaniniPedido.API

dotnet run
# Swagger disponível em: http://localhost:5000
```

### React Client

```bash
cd clients/pedido-react
npm install
npm run dev
# Disponível em: http://localhost:5173
```

### Angular Client

```bash
cd clients/pedido-angular
npm install
ng serve
# Disponível em: http://localhost:4200
```

## Testes Unitários

```bash
dotnet test tests/StefaniniPedido.Tests --logger "console;verbosity=normal"
```
---

## Tecnologias Utilizadas

**Back-end:**
- .NET 10, ASP.NET Core Web API
- Entity Framework Core 10 (SQL Server + InMemory)
- xUnit, Moq, FluentAssertions
- Swashbuckle (Swagger)

**Front-end Angular:**
- Angular 19, TypeScript, Bootstrap 5
- Angular Reactive Forms, HttpClientModule

**Front-end React:**
- React 18, TypeScript, Vite, Bootstrap 5
- React Router DOM v6, React Hook Form, Axios

**Infraestrutura:**
- Docker, Docker Compose
- SQL Server 2022 (container)
- Nginx (reverse proxy para os clientes)

---

## Princípios Aplicados

- **Clean Architecture**: Separação total entre camadas sem dependências invertidas
- **DDD**: Entidades com comportamento, Value Objects, Domain Services
- **SOLID**: SRP (cada classe tem uma responsabilidade), OCP, LSP, ISP, DIP
- **Clean Code**: Nomenclatura descritiva em português, métodos pequenos, sem comentários desnecessários
- **REST**: Verbos corretos, status codes semânticos, recursos nomeados no plural
