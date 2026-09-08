using Gestao_Franquias.Api.DTOs.Fornecedor;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IFornecedorService
{
    Task<IEnumerable<FornecedorResponseDto>> GetAllAsync();
    Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto);
    Task InativarAsync(int id);
}