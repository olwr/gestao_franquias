using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Repositories.Interfaces;

public interface IProdutoRepository : IRepository<ProdutoServico>
{
    Task<IEnumerable<ProdutoServico>> BuscarAsync(string? nome, int? categoriaId, bool? ativo);
    Task<ProdutoServico?> GetComRelacionamentosAsync(int id);
}

public interface ICategoriaRepository : IRepository<Categoria>
{
}