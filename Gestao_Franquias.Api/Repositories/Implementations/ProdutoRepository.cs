using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Repositories.Implementations;

public class ProdutoRepository(ApplicationDbContext context) : Repository<ProdutoServico>(context), IProdutoRepository
{
    public async Task<IEnumerable<ProdutoServico>> BuscarAsync(string? nome, int? categoriaId, bool? ativo)
    {
        var query = DbSet.Include(p => p.Categoria).Include(p => p.Fornecedor).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(p => p.Nome.Contains(nome));
        if (categoriaId.HasValue)
            query = query.Where(p => p.CategoriaId == categoriaId.Value);
        if (ativo.HasValue)
            query = query.Where(p => p.Ativo == ativo.Value);

        return await query.OrderBy(p => p.Nome).ToListAsync();
    }

    public async Task<ProdutoServico?> GetComRelacionamentosAsync(int id) =>
        await DbSet.Include(p => p.Categoria).Include(p => p.Fornecedor)
            .FirstOrDefaultAsync(p => p.Id == id);
}

public class CategoriaRepository(ApplicationDbContext context) : Repository<Categoria>(context), ICategoriaRepository;