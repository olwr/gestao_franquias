using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Repositories.Interfaces;

public interface IVendaRepository : IRepository<Venda>
{
    Task<Venda?> GetComItensAsync(int id);
    Task<IEnumerable<Venda>> ListarPorUnidadeEPeriodoAsync(int unidadeId, DateTime? inicio, DateTime? fim);
}