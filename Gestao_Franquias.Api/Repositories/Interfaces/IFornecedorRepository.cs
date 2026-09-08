using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Repositories.Interfaces;

public interface IFornecedorRepository : IRepository<Fornecedor>
{
    Task<Fornecedor?> GetByCnpjAsync(string cnpj);
}