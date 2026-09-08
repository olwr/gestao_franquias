using Gestao_Franquias.Api.DTOs.Venda;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IVendaService
{
    Task<VendaResponseDto> CriarAsync(VendaCreateDto dto);
    Task<IEnumerable<VendaResponseDto>> ListarPorUnidadeAsync(int unidadeId, DateTime? inicio, DateTime? fim);
}