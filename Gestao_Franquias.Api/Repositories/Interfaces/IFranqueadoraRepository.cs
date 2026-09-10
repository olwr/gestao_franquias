using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Repositories.Interfaces;

public interface IFranqueadoraRepository : IRepository<Franqueadora>
{
    Task<Franqueadora?> GetByCnpjAsync(string cnpj);
}