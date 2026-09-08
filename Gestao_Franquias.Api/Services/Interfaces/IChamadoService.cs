using Gestao_Franquias.Api.DTOs.Chamado;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IChamadoService
{
    Task<ChamadoResponseDto> CriarAsync(ChamadoCreateDto dto);
    Task<IEnumerable<ChamadoResponseDto>> ListarPorUnidadeAsync(int unidadeId);
    Task AtualizarStatusAsync(int id, AtualizarStatusDto dto);
}