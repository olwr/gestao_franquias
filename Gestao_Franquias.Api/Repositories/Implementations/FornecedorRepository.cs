using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Repositories.Implementations;

public class FornecedorRepository(ApplicationDbContext context) : Repository<Fornecedor>(context), IFornecedorRepository
{
    public async Task<Fornecedor?> GetByCnpjAsync(string cnpj) =>
        await DbSet.FirstOrDefaultAsync(f => f.Cnpj == cnpj);
}