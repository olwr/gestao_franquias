using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Repositories.Implementations;

public class FranqueadoraRepository(ApplicationDbContext context)
    : Repository<Franqueadora>(context), IFranqueadoraRepository
{
    public async Task<Franqueadora?> GetByCnpjAsync(string cnpj) =>
        await DbSet.FirstOrDefaultAsync(f => f.Cnpj == cnpj);
}