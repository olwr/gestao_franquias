using Gestao_Franquias.Api.DTOs.Produto;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDto>> BuscarAsync(string? nome, int? categoriaId, bool? ativo);
    Task<ProdutoResponseDto?> ObterPorIdAsync(int id);
    Task<ProdutoResponseDto> CriarAsync(ProdutoCreateDto dto);
    Task InativarAsync(int id);
}

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDto>> GetAllAsync();
    Task<CategoriaResponseDto> CriarAsync(CategoriaCreateDto dto);
}