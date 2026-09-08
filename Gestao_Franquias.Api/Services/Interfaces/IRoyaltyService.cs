using Gestao_Franquias.Api.DTOs.Royalty;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IRoyaltyService
{
    Task<RoyaltyResponseDto> CalcularParaPeriodoAsync(int unidadeId, DateTime periodoReferencia);
    Task<IEnumerable<RoyaltyResponseDto>> ListarPorUnidadeAsync(int unidadeId);
    Task RegistrarPagamentoAsync(int id, RegistrarPagamentoDto dto);
}