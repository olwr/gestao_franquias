using AutoMapper;
using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.DTOs.Venda;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class VendaService(
    ApplicationDbContext context,
    IVendaRepository vendaRepo,
    IEstoqueRepository estoqueRepo,
    Repositories.Interfaces.IRepository<UnidadeFranqueada> unidadeRepo,
    IMapper mapper)
    : IVendaService
{
    public async Task<VendaResponseDto> CriarAsync(VendaCreateDto dto)
    {
        var unidade = await unidadeRepo.GetByIdAsync(dto.UnidadeFranqueadaId)
                      ?? throw new NotFoundException("Unidade não encontrada.");

        if (!unidade.Ativo)
            throw new BusinessException("Não é possível registrar vendas para uma unidade inativa.");

        if (dto.Itens.Count == 0)
            throw new BusinessException("A venda deve conter ao menos um item.");

        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var itensVenda = new List<ItemVenda>();
            decimal valorTotal = 0;

            foreach (var itemDto in dto.Itens)
            {
                var produto = await context.Produtos.FindAsync(itemDto.ProdutoServicoId)
                              ?? throw new NotFoundException($"Produto {itemDto.ProdutoServicoId} não encontrado.");

                var estoque = await estoqueRepo.ObterAsync(dto.UnidadeFranqueadaId, itemDto.ProdutoServicoId)
                              ?? throw new BusinessException(
                                  $"Produto '{produto.Nome}' não possui estoque cadastrado nesta unidade.");

                if (estoque.SaldoAtual < itemDto.Quantidade)
                    throw new BusinessException(
                        $"Estoque insuficiente para '{produto.Nome}'. Saldo atual: {estoque.SaldoAtual}, solicitado: {itemDto.Quantidade}.");

                estoque.SaldoAtual -= itemDto.Quantidade;
                estoqueRepo.Update(estoque);

                context.Movimentacoes.Add(new MovimentacaoEstoque
                {
                    EstoqueId = estoque.Id,
                    Tipo = TipoMovimentacao.Saida,
                    Quantidade = itemDto.Quantidade,
                    DataHora = DateTime.UtcNow,
                    Motivo = "Venda"
                });

                var itemVenda = new ItemVenda
                {
                    ProdutoServicoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoBase
                };
                itensVenda.Add(itemVenda);
                valorTotal += itemDto.Quantidade * produto.PrecoBase;
            }

            var venda = new Venda
            {
                UnidadeFranqueadaId = dto.UnidadeFranqueadaId,
                UsuarioId = dto.UsuarioId,
                DataHora = DateTime.UtcNow,
                ValorTotal = valorTotal,
                Itens = itensVenda
            };

            await vendaRepo.AddAsync(venda);
            await vendaRepo.SaveChangesAsync();
            await transaction.CommitAsync();

            var vendaCompleta = await vendaRepo.GetComItensAsync(venda.Id);
            return mapper.Map<VendaResponseDto>(vendaCompleta);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<VendaResponseDto>> ListarPorUnidadeAsync(int unidadeId, DateTime? inicio,
        DateTime? fim)
    {
        var vendas = await vendaRepo.ListarPorUnidadeEPeriodoAsync(unidadeId, inicio, fim);
        return vendas.Select(v => mapper.Map<VendaResponseDto>(v));
    }
}