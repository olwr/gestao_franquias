using Gestao_Franquias.Api.DTOs.Unidade;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IUnidadeService
{
    Task<IEnumerable<UnidadeResponseDto>>
        BuscarAsync(string? nome, string? cidade, bool? ativo, int page, int pageSize);

    Task<UnidadeResponseDto?> ObterPorIdAsync(int id);
    Task<UnidadeResponseDto> CriarAsync(UnidadeCreateDto dto);
    Task AtualizarAsync(int id, UnidadeUpdateDto dto);
    Task InativarAsync(int id);
}