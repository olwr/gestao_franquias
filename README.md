# Sistema de Gestão de Franquias

![status](https://img.shields.io/badge/Status-Ativo-58919B)
![csharp](https://img.shields.io/badge/C%23-ASP.NET%20Core%209.0-626D74)
![db](https://img.shields.io/badge/PostgreSQL-16-C6B89C)
![docker](https://img.shields.io/badge/Docker-29.7-272725)
![swagger](https://img.shields.io/badge/Swagger-9.0-B7A37F)
![api](https://img.shields.io/badge/API-Insomnia-808B97)
![ide](https://img.shields.io/badge/IDE-JetBrains%20Rider-272727)
![license](https://img.shields.io/badge/License-MIT-C6B895)

## Descrição do projeto

API REST em C# para gestão de uma rede de franquias. O sistema centraliza informações da franqueadora e de suas
unidades, permitindo administrar usuários, franqueadoras, unidades franqueadas, franqueados, categorias, produtos e
serviços, estoque, vendas, fornecedores, royalties e chamados de suporte. O projeto é um caso de estudo para
demonstrar o conhecimento adquirido na disciplina de Desenvolvimento Back-end.

## Objetivo

Demonstrar, simulando um ambiente corporativo real, a construção do backend de um projeto completo: arquitetura em
camadas, banco de dados relacional, autenticação e autorização por perfil, validações, regras de negócio, consultas
gerenciais e documentação da API — com código e API disponíveis para testes e avaliação.

## Pré-requisitos

- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) e Docker Compose
- (Opcional, para desenvolvimento) [JetBrains Rider](https://www.jetbrains.com/rider/) ou outra IDE .NET
- (Opcional, para testes manuais) [Insomnia](https://insomnia.rest/)

## Tecnologias utilizadas

| Camada              | Tecnologia                                                         |
|---------------------|--------------------------------------------------------------------|
| Backend             | C#, ASP.NET Core 9.0, Entity Framework Core, API REST em JSON      |
| Banco de dados      | PostgreSQL 16                                                      |
| Autenticação        | JWT Bearer, Perfis (Roles: Administrador, GestorUnidade, Operador) |
| Testes/documentação | Swagger (Swashbuckle), Insomnia, arquivo `.http`                   |
| Containers          | Docker, Docker Compose                                             |
| Versionamento       | Git, GitHub                                                        |
| Ambiente de dev.    | Ubuntu (WSL2), JetBrains Rider                                     |

## Arquitetura

```
Requisição HTTP
   → Controller (validação de entrada, códigos HTTP, chama Service)
      → Service (regras de negócio, orquestração, cálculos)
         → Repository (acesso a dados via EF Core, LINQ, async/await)
            → PostgreSQL
```

DTOs entram e saem nas bordas do Controller; os `Models`/`Entities` nunca são expostos diretamente na API (evita
over-posting e vazamento de detalhes internos). Regras de negócio (unicidade de CNPJ/e-mail, bloqueio de estoque
negativo, cálculo de venda no servidor, inativação lógica, etc.) vivem exclusivamente na camada de `Services`.

## Configuração de segredos (.env e User Secrets)

O projeto usa duas fontes de configuração sensível, dependendo de como você está executando a API:

### Rodando via Docker Compose

Os segredos (credenciais do banco e chave JWT) ficam no arquivo `.env`, na raiz do repositório, e são repassados como
variáveis de ambiente para os containers pelo próprio `docker-compose.yaml`. Um `.env.example` é versionado como
modelo — copie e ajuste se necessário:

```bash
cp .env.example .env
```

> Neste projeto, tanto `.env` quanto `.env.example` contêm valores fictícios de desenvolvimento (não são credenciais
> reais de produção), então ambos podem ser versionados sem risco. Em um cenário de produção real, `.env` **nunca**
> deveria ser commitado — apenas o `.env.example` com placeholders.

### Rodando localmente via `dotnet run` (sem Docker)

O arquivo `.env` **não é lido automaticamente** pelo .NET fora do Docker. Para desenvolvimento local, os mesmos
segredos são configurados via [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets), que ficam
fora do repositório (armazenados em `~/.microsoft/usersecrets/<UserSecretsId>/secrets.json`, nunca versionados):

```bash
cd Gestao_Franquias.Api

dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=franquias_db;Username=franquias_user;Password=franquias_pass"
dotnet user-secrets set "Jwt:Key" "f89c70c4872b6e9125d5a83dc862bf79af0fa18206134fe26cf7f880f4b5ae3a"
dotnet user-secrets set "Jwt:Issuer" "Gestao_Franquias.Api"
dotnet user-secrets set "Jwt:Audience" "Gestao_Franquias.Api.Clients"
```

O ASP.NET Core aplica a seguinte ordem de precedência de configuração (a de baixo sobrescreve a de cima):

```
appsettings.json → appsettings.Development.json → User Secrets → variáveis de ambiente → argumentos de linha de comando
```

Por isso `appsettings.Development.json` fica com os campos de segredo vazios (`""`) — eles são preenchidos em tempo
de execução pelos User Secrets (execução local) ou pelas variáveis de ambiente do `docker-compose.yaml` (execução em
container).

## Como executar (Docker — recomendado)

```bash
git clone <url-do-repositorio>
cd Gestao_Franquias
cp .env.example .env   # ajuste os valores se necessário
docker compose up -d --build
```

A API sobe já com as migrations aplicadas e o banco populado (seed), sem passos manuais adicionais.

- API: http://localhost:5016
- Swagger: http://localhost:5016/swagger

Para acompanhar os logs (útil para confirmar que as migrations e o seed rodaram sem erro):

```bash
docker compose logs -f api
```

Para parar e remover os containers (mantendo o volume do banco):

```bash
docker compose down
```

Para resetar tudo do zero, incluindo os dados do banco:

```bash
docker compose down -v
docker compose up -d --build
```

## Como executar (local, sem Docker para a API)

```bash
docker compose up -d postgres      # só o banco, em container
cd Gestao_Franquias.Api
dotnet restore
dotnet ef database update          # aplica as migrations (requer dotnet-ef instalado)
dotnet run
```

A porta local é definida em `Properties/launchSettings.json` (por padrão, algo como `http://localhost:5221`) — confira
a porta exibida no terminal ao rodar `dotnet run` e ajuste as URLs de teste (`.http`, Insomnia) de acordo.

## Usuários de teste (populados pelo seed)

| Perfil            | E-mail                          | Senha         |
| ----------------- | ------------------------------- | ------------- |
| Administrador     | admin@franquias.com             | SenhaForte123 |
| Gestor de Unidade | gestor.centro@franquias.com     | SenhaForte123 |
| Operador          | operador.shopping@franquias.com | SenhaForte123 |

O seed também cria uma franqueadora, três unidades (duas ativas, uma inativa — para testar a regra de bloqueio de
vendas), categorias, produtos, fornecedores, estoque inicial (incluindo um item propositalmente abaixo do mínimo),
uma venda de exemplo, registros de royalty (um pago, um pendente) e chamados de suporte em status variados.

## Testando a API

### Via Swagger

1. Acesse `/swagger` com a API em execução.
2. Expanda **Auth → POST /api/auth/login** e execute com um dos usuários de teste acima.
3. Copie o `token` retornado.
4. Clique no botão **Authorize** (cadeado, no topo da página) e cole apenas o token (sem o prefixo `Bearer`).
5. Todos os demais endpoints passam a ser executados autenticados como aquele usuário — o Swagger já mostra quais
   perfis cada rota exige, através dos atributos `[Authorize(Roles = "...")]` documentados.

### Via Insomnia

1. Importe a coleção em `insomnia/Gestao_Franquias-API.insomnia.json`
   (**Application → Preferences → Data → Import Data → From File**).
2. A coleção já vem organizada por módulo (Auth, Usuários, Franqueadoras, Unidades, Categorias e Produtos,
   Fornecedores, Estoque, Vendas, Royalties, Chamados, Relatórios, Cenários de Erro), com os corpos de requisição já
   preenchidos com dados compatíveis com o seed.
3. Rode as três requisições da pasta **Auth** e copie cada `token` retornado para as variáveis de ambiente
   correspondentes (`token_admin`, `token_gestor`, `token_operador`), em **Manage Environments**.
4. As demais requisições já usam essas variáveis no cabeçalho `Authorization: Bearer`.

### Via arquivo `.http` (Rider / VS Code com extensão REST Client)

O arquivo `Gestao_Franquias.Api/Gestao_Franquias.Api.http` contém o mesmo roteiro de testes da coleção Insomnia, mas
com **encadeamento automático de token**: execute os três blocos de login primeiro (clicando no ícone ▶ ao lado de
cada um, na ordem em que aparecem no arquivo) — cada um roda um script (`> {% client.global.set(...) %}`) que guarda
o token numa variável global da sessão. As demais requisições já usam essas variáveis automaticamente, sem precisar
copiar/colar nada manualmente. Ajuste a variável `@baseUrl` no topo do arquivo conforme a porta em uso.

## Estrutura do projeto

```
Gestao_Franquias/
├── Gestao_Franquias.Api/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── UsuariosController.cs
│   │   ├── FranqueadorasController.cs
│   │   ├── UnidadesController.cs
│   │   ├── ProdutosController.cs        (inclui CategoriasController)
│   │   ├── EstoquesController.cs
│   │   ├── VendasController.cs
│   │   ├── RoyaltiesController.cs
│   │   ├── FornecedoresController.cs
│   │   ├── ChamadosController.cs
│   │   └── RelatoriosController.cs
│   ├── Models/                          # Entidades mapeadas pelo EF Core
│   │   ├── Usuario.cs / Perfil.cs
│   │   ├── Franqueadora.cs
│   │   ├── UnidadeFranqueada.cs
│   │   ├── Franqueado.cs
│   │   ├── Categoria.cs / ProdutoServico.cs
│   │   ├── Estoque.cs / MovimentacaoEstoque.cs
│   │   ├── Venda.cs / ItemVenda.cs
│   │   ├── Fornecedor.cs
│   │   ├── Royalty.cs
│   │   └── ChamadoSuporte.cs
│   ├── DTOs/                            # Auth, Usuario, Franqueadora, Unidade, Produto,
│   │                                     # Estoque, Venda, Royalty, Fornecedor, Chamado, Relatorios
│   ├── Services/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   └── Implementations/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Seed/DbSeeder.cs
│   ├── Mappings/AutoMapperProfile.cs
│   ├── Middleware/ (ErrorHandlingMiddleware.cs, Exceptions.cs)
│   ├── Configurations/JwtSettings.cs
│   ├── Migrations/                      # geradas pelo EF Core
│   ├── appsettings.json / appsettings.Development.json
│   ├── Dockerfile
│   ├── Gestao_Franquias.Api.http
│   └── Gestao_Franquias.Api.csproj
├── insomnia/
│   └── Gestao_Franquias-API.insomnia.json
├── docker-compose.yaml
├── .env.example
├── .gitignore / .dockerignore
├── LICENSE
└── Gestao_Franquias.sln
```

## Principais Regras de Negócio

### 1. Usuários / Autenticação

- E-mail único; inativação lógica impede login; senhas sempre hasheadas (BCrypt).

### 2. Franqueadoras

- CNPJ único; inativação lógica (nunca exclusão física).

### 3. Unidades franqueadas

- CNPJ único; inativação lógica; **unidade inativa não pode registrar vendas**.

### 4. Produtos/Serviços e Categorias

- CRUD com filtros por nome, categoria e status; inativação em vez de exclusão física quando há vendas associadas.

### 5. Estoque

- Um registro de `Estoque` por (unidade, produto), criado automaticamente na primeira movimentação.
- Toda alteração de saldo passa por `MovimentacaoEstoque` (Entrada/Saída) — nunca por update direto do saldo.
- Bloqueio de saldo negativo antes de qualquer saída.

### 6. Vendas

- Pertence a uma única unidade; exige ao menos 1 item; `ValorTotal` sempre calculado no servidor.
- Confirmação transacional: valida estoque de cada item, debita estoque, registra movimentações e persiste a venda
  atomicamente — se qualquer etapa falhar, tudo é revertido.

### 7. Royalties

- Percentual configurado por unidade; cálculo por período a partir do faturamento de vendas; status
  Pendente/Pago/Atrasado.

### 8. Fornecedores

- CNPJ único; associação opcional a produtos.

### 9. Chamados de suporte

- Categoria, prioridade e status controlados por enum; `DataEncerramento` preenchida automaticamente ao encerrar.

## Autores

Oliver Benites (RU 5058029)