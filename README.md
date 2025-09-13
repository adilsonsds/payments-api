# Payments API

Este projeto é uma solução ASP.NET Core para gerenciamento de pagamentos, estruturada em múltiplas camadas seguindo boas práticas de arquitetura.

## Estrutura do Projeto

- **Payments.Api**: Camada de apresentação (API REST).
- **Payments.Application**: Camada de aplicação, responsável pela lógica de negócios e padrões CQRS.
- **Payments.Domain**: Camada de domínio, contém as entidades e regras de negócio.
- **Payments.Infra**: Camada de infraestrutura, responsável pelo acesso a dados (EF Core, Npgsql).

```
src/
  Payments.sln
  Payments.Api/
    Controllers/
    Program.cs
    appsettings.json
    ...
  Payments.Application/
    Commands/
    Queries/
    CqrsDispatcher.cs
    ...
  Payments.Domain/
    Payment.cs
    Profile.cs
    ...
  Payments.Infra/
    PaymentsDbContext.cs
    ...
```

## Arquitetura

- **CQRS (Command Query Responsibility Segregation)**: Separação entre comandos (escrita) e queries (leitura), implementado em [`Payments.Application`](Payments.Application/).
- **Entity Framework Core**: Utilizado para persistência de dados, configurado em [`PaymentsDbContext`](Payments.Infra/PaymentsDbContext.cs).
- **Injeção de Dependências**: Utilizada para desacoplamento entre camadas, configurada em [`Program.cs`](Payments.Api/Program.cs).
- **API REST**: Controladores em [`Controllers`](Payments.Api/Controllers/) expõem endpoints HTTP.

## Como Executar

1. **Pré-requisitos**
   - [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
   - Banco de dados PostgreSQL configurado

2. **Configuração**
   - Edite o arquivo [`appsettings.json`](Payments.Api/appsettings.json) com a string de conexão do banco de dados.

3. **Restaurar Dependências**
   ```sh
   dotnet restore src/Payments.sln
   ```

4. **Build**
   ```sh
   dotnet build src/Payments.sln
   ```

5. **Executar a API**
   ```sh
   dotnet run --project src/Payments.Api/Payments.Api.csproj
   ```

6. **Testar Endpoints**
   - Acesse `http://localhost:5054` (ou porta configurada) para utilizar a API.

## Migrations de Banco de Dados

Para gerenciar o esquema do banco de dados, o projeto utiliza o Entity Framework Core Migrations.

### Criando uma Nova Migração

Quando houver alterações nos modelos de domínio (em `Payments.Domain`), é necessário criar uma nova migração para refletir essas mudanças no banco de dados.

```sh
dotnet ef migrations add <NomeDaMigracao> --project src/Payments.Infra --startup-project src/Payments.Api
```
Substitua `<NomeDaMigracao>` por um nome descritivo (ex: `AddUserRoles`).

### Aplicando Migrações

Para aplicar as migrações pendentes e atualizar o esquema do banco de dados:

```sh
dotnet ef database update --project src/Payments.Infra --startup-project src/Payments.Api
```

### Recriando o Banco de Dados

Para limpar e recriar o banco de dados do zero, execute os seguintes comandos.

**Aviso**: O comando a seguir excluirá todos os dados do banco de dados.

1.  **Remover o banco de dados:**
    ```sh
    dotnet ef database drop --project src/Payments.Infra --startup-project src/Payments.Api --force
    ```

2.  **Aplicar todas as migrações novamente:**
    ```sh
    dotnet ef database update --project src/Payments.Infra --startup-project src/Payments.Api
    ```

## Observações

- Os comandos e queries estão organizados em pastas separadas dentro de [`Payments.Application`](Payments.Application/Commands/) e [`Payments.Application/Queries`](Payments.Application/Queries/).
- O projeto utiliza boas práticas de separação de responsabilidades, facilitando manutenção e testes.

---