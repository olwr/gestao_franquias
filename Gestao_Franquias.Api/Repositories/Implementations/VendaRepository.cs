using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Repositories.Implementations;

public class VendaRepository(ApplicationDbContext context) : Repository<Venda>(context), IVendaRepository
{
    public async Task<Venda?> GetComItensAsync(int id) =>
        await DbSet.Include(v => v.Itens).ThenInclude(i => i.ProdutoServico)
            .FirstOrDefaultAsync(v => v.Id == id);

    public async Task<IEnumerable<Venda>> ListarPorUnidadeEPeriodoAsync(int unidadeId, DateTime? inicio, DateTime? fim)
    {
        var query = DbSet.Include(v => v.Itens).ThenInclude(i => i.ProdutoServico)
            .Where(v => v.UnidadeFranqueadaId == unidadeId);

        if (inicio.HasValue) query = query.Where(v => v.DataHora >= inicio.Value);
        if (fim.HasValue) query = query.Where(v => v.DataHora <= fim.Value);

        return await query.OrderByDescending(v => v.DataHora).ToListAsync();
    }
}