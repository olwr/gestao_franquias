using AutoMapper;
using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.DTOs.Estoque;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class EstoqueService(IEstoqueRepository estoqueRepo, ApplicationDbContext context, IMapper mapper)
    : IEstoqueService
{
    // usado só para AddAsync de MovimentacaoEstoque diretamente

    public async Task<IEnumerable<EstoqueResponseDto>> ListarPorUnidadeAsync(int unidadeId)
    {
        var estoques = await estoqueRepo.ListarPorUnidadeAsync(unidadeId);
        return estoques.Select(e => mapper.Map<EstoqueResponseDto>(e));
    }

    public async Task<IEnumerable<EstoqueResponseDto>> ListarCriticosAsync()
    {
        var criticos = await estoqueRepo.ListarCriticosAsync();
        return criticos.Select(e => mapper.Map<EstoqueResponseDto>(e));
    }

    public async Task<MovimentacaoResponseDto> RegistrarMovimentacaoAsync(MovimentacaoCreateDto dto)
    {
        if (!Enum.TryParse<TipoMovimentacao>(dto.Tipo, ignoreCase: true, out var tipo))
            throw new BusinessException("Tipo de movimentação inválido. Use 'Entrada' ou 'Saida'.");

        if (dto.Quantidade <= 0)
            throw new BusinessException("Quantidade deve ser maior que zero.");

        var estoque = await estoqueRepo.ObterAsync(dto.UnidadeFranqueadaId, dto.ProdutoServicoId);

        // Cria o registro de estoque na primeira movimentação, se ainda não existir.
        if (estoque is null)
        {
            estoque = new Estoque
            {
                UnidadeFranqueadaId = dto.UnidadeFranqueadaId,
                ProdutoServicoId = dto.ProdutoServicoId,
                SaldoAtual = 0,
                EstoqueMinimo = 0
            };
            await estoqueRepo.AddAsync(estoque);
            await estoqueRepo.SaveChangesAsync();
        }

        if (tipo == TipoMovimentacao.Saida && estoque.SaldoAtual < dto.Quantidade)
            throw new BusinessException(
                $"Estoque insuficiente. Saldo atual: {estoque.SaldoAtual}, solicitado: {dto.Quantidade}.");

        estoque.SaldoAtual += tipo == TipoMovimentacao.Entrada ? dto.Quantidade : -dto.Quantidade;

        var movimentacao = new MovimentacaoEstoque
        {
            EstoqueId = estoque.Id,
            Tipo = tipo,
            Quantidade = dto.Quantidade,
            DataHora = DateTime.UtcNow,
            Motivo = dto.Motivo ?? tipo.ToString()
        };

        context.Movimentacoes.Add(movimentacao);
        estoqueRepo.Update(estoque);
        await estoqueRepo.SaveChangesAsync();

        return mapper.Map<MovimentacaoResponseDto>(movimentacao);
    }
}