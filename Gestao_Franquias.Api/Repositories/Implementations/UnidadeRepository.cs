using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Repositories.Implementations;

public class UnidadeRepository(ApplicationDbContext context)
    : Repository<UnidadeFranqueada>(context), IUnidadeRepository
{
    public async Task<UnidadeFranqueada?> GetByCnpjAsync(string cnpj) =>
        await DbSet.FirstOrDefaultAsync(u => u.Cnpj == cnpj);

    public async Task<IEnumerable<UnidadeFranqueada>> BuscarAsync(
        string? nome, string? cidade, bool? ativo, int page, int pageSize)
    {
        var query = DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(u => u.Nome.Contains(nome));
        if (!string.IsNullOrWhiteSpace(cidade))
            query = query.Where(u => u.Cidade.Contains(cidade));
        if (ativo.HasValue)
            query = query.Where(u => u.Ativo == ativo.Value);

        return await query
            .OrderBy(u => u.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}