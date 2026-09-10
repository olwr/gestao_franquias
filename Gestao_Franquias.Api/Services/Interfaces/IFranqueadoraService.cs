using Gestao_Franquias.Api.DTOs.Franqueadora;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IFranqueadoraService
{
    Task<IEnumerable<FranqueadoraResponseDto>> GetAllAsync();
    Task<FranqueadoraResponseDto?> ObterPorIdAsync(int id);
    Task<FranqueadoraResponseDto> CriarAsync(FranqueadoraCreateDto dto);
    Task AtualizarAsync(int id, FranqueadoraUpdateDto dto);
    Task InativarAsync(int id);
}