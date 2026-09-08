using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Repositories.Interfaces;

public interface IUnidadeRepository : IRepository<UnidadeFranqueada>
{
    Task<UnidadeFranqueada?> GetByCnpjAsync(string cnpj);
    Task<IEnumerable<UnidadeFranqueada>> BuscarAsync(string? nome, string? cidade, bool? ativo, int page, int pageSize);
}