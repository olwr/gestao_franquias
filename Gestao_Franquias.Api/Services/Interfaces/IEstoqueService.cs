using Gestao_Franquias.Api.DTOs.Estoque;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IEstoqueService
{
    Task<IEnumerable<EstoqueResponseDto>> ListarPorUnidadeAsync(int unidadeId);
    Task<IEnumerable<EstoqueResponseDto>> ListarCriticosAsync();
    Task<MovimentacaoResponseDto> RegistrarMovimentacaoAsync(MovimentacaoCreateDto dto);
}