using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Repositories.Interfaces;

public interface IEstoqueRepository : IRepository<Estoque>
{
    Task<Estoque?> ObterAsync(int unidadeId, int produtoId);
    Task<IEnumerable<Estoque>> ListarPorUnidadeAsync(int unidadeId);
    Task<IEnumerable<Estoque>> ListarCriticosAsync();
}