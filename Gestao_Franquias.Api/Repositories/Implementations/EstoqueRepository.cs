using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Repositories.Implementations;

public class EstoqueRepository(ApplicationDbContext context) : Repository<Estoque>(context), IEstoqueRepository
{
    public async Task<Estoque?> ObterAsync(int unidadeId, int produtoId) =>
        await DbSet.Include(e => e.ProdutoServico).Include(e => e.UnidadeFranqueada)
            .FirstOrDefaultAsync(e => e.UnidadeFranqueadaId == unidadeId && e.ProdutoServicoId == produtoId);

    public async Task<IEnumerable<Estoque>> ListarPorUnidadeAsync(int unidadeId) =>
        await DbSet.Include(e => e.ProdutoServico).Include(e => e.UnidadeFranqueada)
            .Where(e => e.UnidadeFranqueadaId == unidadeId)
            .ToListAsync();

    public async Task<IEnumerable<Estoque>> ListarCriticosAsync() =>
        await DbSet.Include(e => e.ProdutoServico).Include(e => e.UnidadeFranqueada)
            .Where(e => e.SaldoAtual < e.EstoqueMinimo)
            .ToListAsync();
}