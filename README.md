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

API REST em CSharp para gestão de uma rede de franquias. O sistema centraliza informações de franquias e suas unidades,
permitindo administrar usuários, franqueadoras, unidades, franqueados, categorias, produtos, serviços, estoque, vendas,
fornecedores, royalties e chamados de suporte. O projeto é um caso de estudo para demonstrar o conhecimento adquirido na disciplina
de desenvolvimento web backend.

## Objetivo

Demonstrar, simulando um ambiente corporativo real, a construção do backend de um projeto completo, incluindo
arquitetura organizada, banco de dados relacional, autenticação, autorização, validações, regras de negócio, consultas e
documentação da API, com código e API disponíveis para testes e avaliação. O sistema desenvolvido fornece uma API
capaz de registrar, consultar, atualizar e excluir informações, além de aplicar regras de negócio e gerar indicadores
úteis à gestão.

## Pré-requisitos
.NET SDK 9.0, Docker e Docker Compose.

## Tecnologias utilizadas

| Camada              | Tecnologia                                                    |
|---------------------|---------------------------------------------------------------|
| Backend             | C#, ASP.NET Core 9.0, Entity Framework Core, API REST em JSON |
| Banco de dados      | PostgreSQL 16                                                 |
| Autenticação        | JWT Bearer, Perfis (Roles)                                    |
| Testes/documentação | Swagger, Insomnia, .http                                      |
| Containers          | Docker, Docker Compose                                        |
| Versionamento       | Git, GitHub                                                   |

## Arquitetura

```
Requisição HTTP
   → Controller (validação de entrada, códigos HTTP, chama Service)
      → Service (regras de negócio, orquestração, cálculos)
         → Repository (acesso a dados via EF Core, LINQ, async/await)
            → PostgreSQL
```

DTOs entram e saem nas bordas do Controller; os `Models`/`Entities` nunca são expostos diretamente na API (evita
over-posting e vazamento de detalhes internos).

## Como executar (Docker — recomendado)
    git clone <url-do-repositorio>
    cd Franquias.Api
    docker compose up -d --build

API disponível em http://localhost:5016 · Swagger em http://localhost:5016/swagger

## Como executar (local, sem Docker para a API)
    docker compose up -d postgres
    dotnet restore
    dotnet ef database update
    dotnet run

## Usuário de teste (seed)
    email: admin@franquias.com
    senha: SenhaForte123

## Testando a API
- Swagger UI: /swagger
- Coleção Insomnia: insomnia/Franquias-API.insomnia.json
- Requisições .http: Franquias.Api.http

## Estrutura do Projeto

```sh
Franquias.Api/
├── Controllers/
│   ├── AuthController.cs
│   ├── UsuariosController.cs
│   ├── FranqueadorasController.cs
│   ├── UnidadesController.cs
│   ├── ProdutosController.cs
│   ├── EstoquesController.cs
│   ├── VendasController.cs
│   ├── RoyaltiesController.cs
│   ├── FornecedoresController.cs
│   ├── ChamadosController.cs
│   └── RelatoriosController.cs
├── Models/                      # Entidades (mapeadas pelo EF Core)
│   ├── Usuario.cs
│   ├── Perfil.cs (enum)
│   ├── Franqueadora.cs
│   ├── UnidadeFranqueada.cs
│   ├── Franqueado.cs
│   ├── Categoria.cs
│   ├── ProdutoServico.cs
│   ├── Estoque.cs
│   ├── MovimentacaoEstoque.cs
│   ├── Venda.cs
│   ├── ItemVenda.cs
│   ├── Fornecedor.cs
│   ├── Royalty.cs
│   └── ChamadoSuporte.cs
├── DTOs/
│   ├── Auth/ (LoginDto, TokenResponseDto)
│   ├── Usuario/ (UsuarioCreateDto, UsuarioResponseDto, ...)
│   ├── Unidade/
│   ├── Produto/
│   ├── Estoque/
│   ├── Venda/
│   ├── Royalty/
│   ├── Fornecedor/
│   ├── Chamado/
│   └── Relatorios/
├── Services/
│   ├── Interfaces/ (IUsuarioService, IUnidadeService, IVendaService, ...)
│   └── Implementations/
├── Repositories/
│   ├── Interfaces/ (IRepository<T>, IUnidadeRepository, IVendaRepository, ...)
│   └── Implementations/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Seed/DbSeeder.cs
├── Mappings/
│   └── AutoMapperProfile.cs
├── Middleware/
│   └── ErrorHandlingMiddleware.cs
├── Configurations/
│   ├── JwtSettings.cs
│   └── SwaggerConfig.cs
├── Migrations/                  # geradas pelo EF Core
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Dockerfile
├── docker-compose.yml
├── Franquias.Api.http
├── Program.cs
└── Franquias.Api.csproj
```

## Principais Regras de Negócio
### 9.1 Usuários / Autenticação

- E-mail único (validação em `CriarAsync`, retornando 400 se duplicado).
- Inativação lógica (`Ativo = false`) impede login.
- Senhas sempre hasheadas (nunca armazenar em texto puro).

### 9.2 Unidades franqueadas

- CNPJ único.
- Inativar (não excluir fisicamente) ao "remover" uma unidade.
- **Unidade inativa não pode registrar novas vendas** — validar no `VendaService.CriarAsync` antes de qualquer outra coisa.

### 9.3 Produtos/Serviços e Categorias

- CRUD padrão; permitir filtro por nome, categoria e status.
- Não permitir excluir fisicamente produto com vendas associadas — inativar em vez disso.

### 9.4 Estoque

- Um registro de `Estoque` por (`UnidadeFranqueadaId`, `ProdutoServicoId`) — criado automaticamente na primeira movimentação, se não existir.
- Toda alteração de saldo passa por `MovimentacaoEstoque` (entrada/saída), nunca por update direto do campo `SaldoAtual` fora do Service.
- **Bloquear saldo negativo**: antes de uma saída (venda ou ajuste manual), verificar `SaldoAtual - Quantidade >= 0`; se não, lançar `BusinessException("Estoque insuficiente para o produto X na unidade Y.")`.
- Endpoint de consulta de itens abaixo do mínimo: `WHERE SaldoAtual < EstoqueMinimo`.

### 9.5 Vendas

- Uma venda pertence a **uma única unidade** e deve ter **pelo menos 1 item** (validar `Itens.Count >= 1` no DTO ou no Service antes de persistir).
- `ValorTotal` é sempre **calculado no servidor** a partir de `Itens.Sum(i => i.Quantidade * i.PrecoUnitario)` — nunca aceito diretamente do cliente.
- Ao confirmar a venda, dentro de uma **transação** (`context.Database.BeginTransactionAsync()`):
    1. Verificar estoque suficiente para cada item.
    2. Debitar o estoque de cada produto na unidade (gerando `MovimentacaoEstoque` do tipo Saída).
    3. Persistir `Venda` e `ItemVenda`.
    4. Commit; se qualquer passo falhar, rollback e retornar erro 400 com mensagem clara.

### 9.6 Royalties / Financeiro

- `PercentualRoyalty` configurado por unidade (campo em `UnidadeFranqueada` ou entidade própria `ConfiguracaoRoyalty`).
- Cálculo por período: `FaturamentoBase = Sum(Venda.ValorTotal)` das vendas da unidade dentro do intervalo de datas; `ValorCalculado = FaturamentoBase * (PercentualAplicado / 100)`.
- `StatusPagamento`: `Pendente`, `Pago`, `Atrasado` — endpoint `PUT /api/royalties/{id}/pagamento` para registrar pagamento (com `DataPagamento`).
- Endpoint de consulta: valores devidos x pagos por unidade/período.

### 9.7 Fornecedores

- CNPJ único; CRUD padrão; filtro por nome/CNPJ/status; associação opcional a produtos.

### 9.8 Chamados de suporte

- Categoria (ex.: Financeiro, Operacional, TI, Suprimentos), Prioridade (Baixa/Média/Alta/Urgente), Status (Aberto, EmAndamento, Resolvido, Encerrado).
- `DataAbertura` automática no `Create`; `DataEncerramento` preenchida ao mudar status para Encerrado.
- Endpoint `PATCH /api/chamados/{id}/status` para atualizar status/adicionar observações sem reenviar o objeto todo.