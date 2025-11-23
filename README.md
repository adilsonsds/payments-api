# Payments API

Este projeto é uma solução ASP.NET Core para gerenciamento de pagamentos, estruturada em múltiplas camadas seguindo boas práticas de arquitetura.

## Estrutura do Projeto

```
src/
├── Payments.sln
│
├── Core/ (Negócio)
│   ├── Payments.Domain/          # Entidades e regras de negócio
│   └── Payments.Application/     # Lógica de aplicação e CQRS
│
├── Infrastructure/ (Específico)
│   └── Payments.Infra/          # Acesso a dados (EF Core, Npgsql)
│
└── Presentation/
    └── Payments.Api/            # API REST e controladores
```

### Responsabilidades por Camada

- **Core**: Contém a lógica de negócio pura e independente de tecnologia
- **Infrastructure**: Implementações específicas de persistência e acesso a dados
- **Presentation**: Interface de usuário e pontos de entrada da aplicação

## Arquitetura

- **CQRS (Command Query Responsibility Segregation)**: Separação entre comandos (escrita) e queries (leitura), implementado em [`Payments.Application`](Payments.Application/).
- **Versionamento de API**: Controladores organizados por versão (v1) para facilitar evolução da API.
- **Entity Framework Core**: Utilizado para persistência de dados, configurado em [`PaymentsDbContext`](Payments.Infra/PaymentsDbContext.cs).
- **Injeção de Dependências**: Utilizada para desacoplamento entre camadas, configurada em [`Program.cs`](Payments.Api/Program.cs).
- **API REST**: Controladores em [`Controllers/v1`](Payments.Api/Controllers/v1/) expõem endpoints HTTP versionados.

### Funcionalidades

O sistema oferece:
- **Gerenciamento de Pagamentos**: CRUD completo de pagamentos
- **Gerenciamento de Perfis**: CRUD completo de perfis de usuários
- **Sistema de Backup**: Integração com Google Drive para backup de dados

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
   A API ficará disponível em:
   - HTTP: `http://localhost:5054`
   - HTTPS: `https://localhost:5055`

6. **Testar Endpoints**
   - Acesse `http://localhost:5054` para utilizar a API.
   - Utilize o arquivo [`Payments.Api.http`](src/Payments.Api/Payments.Api.http) para testar os endpoints com o REST Client do VS Code.

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

- Os comandos e queries estão organizados em pastas separadas dentro de [`Payments.Application/Commands/v1`](Payments.Application/Commands/v1/) e [`Payments.Application/Queries/v1`](Payments.Application/Queries/v1/).
- A API é versionada, com todos os controladores na versão v1 atualmente.
- O projeto utiliza boas práticas de separação de responsabilidades, facilitando manutenção e testes.
- As abstrações compartilhadas permitem fácil extensão e manutenção de integrações externas.
- O sistema de backup está integrado com Google Drive através do serviço `GoogleDriveService`.

## Endpoints Disponíveis

### Payments (v1)
- `GET /api/v1/payments` - Lista todos os pagamentos
- `GET /api/v1/payments/{id}` - Busca pagamento por ID
- `POST /api/v1/payments` - Cria novo pagamento
- `PUT /api/v1/payments/{id}` - Atualiza pagamento
- `DELETE /api/v1/payments/{id}` - Remove pagamento

### Profiles (v1)
- `GET /api/v1/profiles` - Lista todos os perfis
- `GET /api/v1/profiles/{id}` - Busca perfil por ID
- `POST /api/v1/profiles` - Cria novo perfil
- `DELETE /api/v1/profiles/{id}` - Remove perfil

### Backup (v1)
- `POST /api/v1/backup` - Cria backup no Google Drive

---