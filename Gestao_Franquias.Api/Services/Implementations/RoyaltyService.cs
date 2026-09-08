using AutoMapper;
using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.DTOs.Royalty;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Services.Implementations;

public class RoyaltyService(
    ApplicationDbContext context,
    IRepository<Royalty> royaltyRepo,
    IRepository<UnidadeFranqueada> unidadeRepo,
    IMapper mapper)
    : IRoyaltyService
{
    public async Task<RoyaltyResponseDto> CalcularParaPeriodoAsync(int unidadeId, DateTime periodoReferencia)
    {
        var unidade = await unidadeRepo.GetByIdAsync(unidadeId)
                      ?? throw new NotFoundException("Unidade não encontrada.");

        var inicioPeriodo = new DateTime(periodoReferencia.Year, periodoReferencia.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var fimPeriodo = inicioPeriodo.AddMonths(1);

        var faturamento = await context.Vendas
            .Where(v => v.UnidadeFranqueadaId == unidadeId && v.DataHora >= inicioPeriodo && v.DataHora < fimPeriodo)
            .SumAsync(v => (decimal?)v.ValorTotal) ?? 0m;

        var royalty = new Royalty
        {
            UnidadeFranqueadaId = unidadeId,
            PeriodoReferencia = inicioPeriodo,
            PercentualAplicado = unidade.PercentualRoyalty,
            FaturamentoBase = faturamento,
            ValorCalculado = faturamento * (unidade.PercentualRoyalty / 100m),
            StatusPagamento = StatusPagamentoRoyalty.Pendente
        };

        await royaltyRepo.AddAsync(royalty);
        await royaltyRepo.SaveChangesAsync();
        return mapper.Map<RoyaltyResponseDto>(royalty);
    }

    public async Task<IEnumerable<RoyaltyResponseDto>> ListarPorUnidadeAsync(int unidadeId)
    {
        var royalties = await royaltyRepo.FindAsync(r => r.UnidadeFranqueadaId == unidadeId);
        return royalties.OrderByDescending(r => r.PeriodoReferencia)
            .Select(r => mapper.Map<RoyaltyResponseDto>(r));
    }

    public async Task RegistrarPagamentoAsync(int id, RegistrarPagamentoDto dto)
    {
        var royalty = await royaltyRepo.GetByIdAsync(id)
                      ?? throw new NotFoundException($"Royalty {id} não encontrado.");

        royalty.StatusPagamento = StatusPagamentoRoyalty.Pago;
        royalty.DataPagamento = dto.DataPagamento;

        royaltyRepo.Update(royalty);
        await royaltyRepo.SaveChangesAsync();
    }
}