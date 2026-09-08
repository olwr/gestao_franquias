using Gestao_Franquias.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Idempotência: se já existe alguma franqueadora, assume que o seed já rodou.
        if (await context.Franqueadoras.AnyAsync())
            return;

        // 1) Franqueadora ------------------------------------------------
        var franqueadora = new Franqueadora
        {
            RazaoSocial = "Rede Exemplo Franquias Ltda",
            Cnpj = "11111111000191",
            Telefone = "(53) 3000-0000",
            Email = "contato@redeexemplo.com.br"
        };
        context.Franqueadoras.Add(franqueadora);
        await context.SaveChangesAsync(); // gera FranqueadoraId

        // 2) Unidades franqueadas -----------------------------------------
        var unidadeCentro = new UnidadeFranqueada
        {
            Nome = "Unidade Centro",
            Cnpj = "12345678000101",
            Endereco = "Rua das Flores, 100",
            Cidade = "Pelotas",
            DataInicio = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc),
            Ativo = true,
            PercentualRoyalty = 5.00m,
            FranqueadoraId = franqueadora.Id
        };
        var unidadeShopping = new UnidadeFranqueada
        {
            Nome = "Unidade Shopping",
            Cnpj = "12345678000202",
            Endereco = "Av. Bento Gonçalves, 500",
            Cidade = "Pelotas",
            DataInicio = new DateTime(2023, 8, 15, 0, 0, 0, DateTimeKind.Utc),
            Ativo = true,
            PercentualRoyalty = 6.00m,
            FranqueadoraId = franqueadora.Id
        };
        var unidadeInativa = new UnidadeFranqueada
        {
            Nome = "Unidade Antiga (encerrada)",
            Cnpj = "12345678000303",
            Endereco = "Rua Velha, 10",
            Cidade = "Rio Grande",
            DataInicio = new DateTime(2021, 1, 10, 0, 0, 0, DateTimeKind.Utc),
            Ativo = false, // exemplo de inativação lógica
            PercentualRoyalty = 5.00m,
            FranqueadoraId = franqueadora.Id
        };
        context.Unidades.AddRange(unidadeCentro, unidadeShopping, unidadeInativa);
        await context.SaveChangesAsync(); // gera Ids das unidades

        // 3) Franqueados (responsáveis por unidade) ------------------------
        context.Franqueados.AddRange(
            new Franqueado
            {
                Nome = "Ana Beatriz Souza",
                Cpf = "11122233344",
                Telefone = "(53) 99999-0001",
                Email = "ana.souza@redeexemplo.com.br",
                ResponsavelPrincipal = true,
                UnidadeFranqueadaId = unidadeCentro.Id
            },
            new Franqueado
            {
                Nome = "Carlos Eduardo Lima",
                Cpf = "22233344455",
                Telefone = "(53) 99999-0002",
                Email = "carlos.lima@redeexemplo.com.br",
                ResponsavelPrincipal = true,
                UnidadeFranqueadaId = unidadeShopping.Id
            }
        );

        // 4) Categorias ----------------------------------------------------
        var categoriaBebidas = new Categoria { Nome = "Bebidas" };
        var categoriaLanches = new Categoria { Nome = "Lanches" };
        var categoriaSobremesas = new Categoria { Nome = "Sobremesas" };
        context.Categorias.AddRange(categoriaBebidas, categoriaLanches, categoriaSobremesas);
        await context.SaveChangesAsync(); // gera Ids das categorias

        // 5) Fornecedores ----------------------------------------------------
        var fornecedorBebidas = new Fornecedor
        {
            Nome = "Distribuidora Sul Bebidas",
            Cnpj = "33344455000166",
            Contato = "(53) 3222-1000",
            Ativo = true
        };
        var fornecedorAlimentos = new Fornecedor
        {
            Nome = "Alimentos & Cia Distribuição",
            Cnpj = "44455566000177",
            Contato = "(53) 3222-2000",
            Ativo = true
        };
        context.Fornecedores.AddRange(fornecedorBebidas, fornecedorAlimentos);
        await context.SaveChangesAsync(); // gera Ids dos fornecedores

        // 6) Produtos/Serviços ----------------------------------------------
        var refrigerante = new ProdutoServico
        {
            Nome = "Refrigerante Lata 350ml",
            Descricao = "Refrigerante gelado, diversos sabores",
            PrecoBase = 6.50m,
            Ativo = true,
            CategoriaId = categoriaBebidas.Id,
            FornecedorId = fornecedorBebidas.Id
        };
        var suco = new ProdutoServico
        {
            Nome = "Suco Natural 500ml",
            Descricao = "Suco natural da fruta, sem conservantes",
            PrecoBase = 9.00m,
            Ativo = true,
            CategoriaId = categoriaBebidas.Id,
            FornecedorId = fornecedorBebidas.Id
        };
        var hamburguer = new ProdutoServico
        {
            Nome = "Hambúrguer Artesanal",
            Descricao = "Pão brioche, blend 150g, queijo e salada",
            PrecoBase = 24.90m,
            Ativo = true,
            CategoriaId = categoriaLanches.Id,
            FornecedorId = fornecedorAlimentos.Id
        };
        var batataFrita = new ProdutoServico
        {
            Nome = "Batata Frita Porção",
            Descricao = "Porção individual de batata frita",
            PrecoBase = 15.00m,
            Ativo = true,
            CategoriaId = categoriaLanches.Id,
            FornecedorId = fornecedorAlimentos.Id
        };
        var brownie = new ProdutoServico
        {
            Nome = "Brownie de Chocolate",
            Descricao = "Fatia individual, com nozes",
            PrecoBase = 12.00m,
            Ativo = true,
            CategoriaId = categoriaSobremesas.Id,
            FornecedorId = fornecedorAlimentos.Id
        };
        context.Produtos.AddRange(refrigerante, suco, hamburguer, batataFrita, brownie);
        await context.SaveChangesAsync(); // gera Ids dos produtos

        // 7) Usuários (um por perfil, senha hasheada com BCrypt) -----------
        var senhaHashPadrao = BCrypt.Net.BCrypt.HashPassword("SenhaForte123");

        var admin = new Usuario
        {
            Nome = "Administrador Geral",
            Email = "admin@franquias.com",
            SenhaHash = senhaHashPadrao,
            Perfil = Perfil.Administrador,
            Ativo = true,
            UnidadeFranqueadaId = null // administrador não pertence a uma unidade específica
        };
        var gestorCentro = new Usuario
        {
            Nome = "Fernanda Gestora",
            Email = "gestor.centro@franquias.com",
            SenhaHash = senhaHashPadrao,
            Perfil = Perfil.GestorUnidade,
            Ativo = true,
            UnidadeFranqueadaId = unidadeCentro.Id
        };
        var operadorShopping = new Usuario
        {
            Nome = "Rafael Operador",
            Email = "operador.shopping@franquias.com",
            SenhaHash = senhaHashPadrao,
            Perfil = Perfil.Operador,
            Ativo = true,
            UnidadeFranqueadaId = unidadeShopping.Id
        };
        context.Usuarios.AddRange(admin, gestorCentro, operadorShopping);
        await context.SaveChangesAsync(); // gera Ids dos usuários

        // 8) Estoque inicial por unidade -------------------------------------
        var produtosParaEstoque = new[] { refrigerante, suco, hamburguer, batataFrita, brownie };
        var unidadesAtivas = new[] { unidadeCentro, unidadeShopping };

        var estoques = new List<Estoque>();
        foreach (var unidade in unidadesAtivas)
        {
            foreach (var produto in produtosParaEstoque)
            {
                estoques.Add(new Estoque
                {
                    UnidadeFranqueadaId = unidade.Id,
                    ProdutoServicoId = produto.Id,
                    SaldoAtual = 50, // saldo inicial de exemplo
                    EstoqueMinimo = 10
                });
            }
        }

        context.Estoques.AddRange(estoques);
        await context.SaveChangesAsync(); // gera Ids dos estoques

        // Deixa um item propositalmente abaixo do mínimo, para exercitar
        // o endpoint de "estoque crítico" (seção 10 do guia).
        var estoqueBrownieShopping = estoques.First(e =>
            e.UnidadeFranqueadaId == unidadeShopping.Id && e.ProdutoServicoId == brownie.Id);
        estoqueBrownieShopping.SaldoAtual = 3; // abaixo do EstoqueMinimo (10)

        // 9) Venda de exemplo (Unidade Centro) + débito de estoque -----------
        var estoqueRefriCentro = estoques.First(e =>
            e.UnidadeFranqueadaId == unidadeCentro.Id && e.ProdutoServicoId == refrigerante.Id);
        var estoqueHamburguerCentro = estoques.First(e =>
            e.UnidadeFranqueadaId == unidadeCentro.Id && e.ProdutoServicoId == hamburguer.Id);

        const int qtdRefri = 2;
        const int qtdHamburguer = 1;

        var vendaExemplo = new Venda
        {
            UnidadeFranqueadaId = unidadeCentro.Id,
            UsuarioId = gestorCentro.Id,
            DataHora = DateTime.UtcNow.AddDays(-2),
            ValorTotal = qtdRefri * refrigerante.PrecoBase + qtdHamburguer * hamburguer.PrecoBase,
            Itens = new List<ItemVenda>
            {
                new()
                {
                    ProdutoServicoId = refrigerante.Id,
                    Quantidade = qtdRefri,
                    PrecoUnitario = refrigerante.PrecoBase
                },
                new()
                {
                    ProdutoServicoId = hamburguer.Id,
                    Quantidade = qtdHamburguer,
                    PrecoUnitario = hamburguer.PrecoBase
                }
            }
        };
        context.Vendas.Add(vendaExemplo);

        // Debita o estoque e registra as movimentações correspondentes,
        // exatamente como o VendaService faria em uma venda real (seção 9.5).
        estoqueRefriCentro.SaldoAtual -= qtdRefri;
        estoqueHamburguerCentro.SaldoAtual -= qtdHamburguer;

        context.Movimentacoes.AddRange(
            new MovimentacaoEstoque
            {
                EstoqueId = estoqueRefriCentro.Id,
                Tipo = TipoMovimentacao.Saida,
                Quantidade = qtdRefri,
                DataHora = vendaExemplo.DataHora,
                Motivo = "Venda"
            },
            new MovimentacaoEstoque
            {
                EstoqueId = estoqueHamburguerCentro.Id,
                Tipo = TipoMovimentacao.Saida,
                Quantidade = qtdHamburguer,
                DataHora = vendaExemplo.DataHora,
                Motivo = "Venda"
            }
        );

        // 10) Royalty do mês anterior para as duas unidades ativas -----------
        var periodoReferencia = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddMonths(-1);

        context.Royalties.AddRange(
            new Royalty
            {
                UnidadeFranqueadaId = unidadeCentro.Id,
                PeriodoReferencia = periodoReferencia,
                PercentualAplicado = unidadeCentro.PercentualRoyalty,
                FaturamentoBase = 4200.00m,
                ValorCalculado = 4200.00m * (unidadeCentro.PercentualRoyalty / 100m),
                StatusPagamento = StatusPagamentoRoyalty.Pago,
                DataPagamento = periodoReferencia.AddMonths(1).AddDays(5)
            },
            new Royalty
            {
                UnidadeFranqueadaId = unidadeShopping.Id,
                PeriodoReferencia = periodoReferencia,
                PercentualAplicado = unidadeShopping.PercentualRoyalty,
                FaturamentoBase = 6100.00m,
                ValorCalculado = 6100.00m * (unidadeShopping.PercentualRoyalty / 100m),
                StatusPagamento = StatusPagamentoRoyalty.Pendente,
                DataPagamento = null
            }
        );

        // 11) Chamados de suporte de exemplo ---------------------------------
        context.Chamados.AddRange(
            new ChamadoSuporte
            {
                UnidadeFranqueadaId = unidadeCentro.Id,
                Categoria = CategoriaChamado.Operacional,
                Prioridade = PrioridadeChamado.Media,
                Status = StatusChamado.Aberto,
                Descricao = "Máquina de cartão apresentando falhas intermitentes.",
                DataAbertura = DateTime.UtcNow.AddDays(-1)
            },
            new ChamadoSuporte
            {
                UnidadeFranqueadaId = unidadeShopping.Id,
                Categoria = CategoriaChamado.Suprimentos,
                Prioridade = PrioridadeChamado.Alta,
                Status = StatusChamado.EmAndamento,
                Descricao = "Atraso na entrega de insumos do fornecedor de alimentos.",
                DataAbertura = DateTime.UtcNow.AddDays(-3)
            },
            new ChamadoSuporte
            {
                UnidadeFranqueadaId = unidadeCentro.Id,
                Categoria = CategoriaChamado.TI,
                Prioridade = PrioridadeChamado.Baixa,
                Status = StatusChamado.Resolvido,
                Descricao = "Solicitação de acesso ao sistema para novo operador.",
                DataAbertura = DateTime.UtcNow.AddDays(-10),
                DataEncerramento = DateTime.UtcNow.AddDays(-9)
            }
        );

        await context.SaveChangesAsync();
    }
}