using Gestao_Franquias.Api.DTOs.Royalty;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/royalties")]
[Authorize(Roles = "Administrador")]
public class RoyaltiesController(IRoyaltyService service) : ControllerBase
{
    [HttpPost("calcular")]
    public async Task<ActionResult<RoyaltyResponseDto>> Calcular(
        [FromQuery] int unidadeId, [FromQuery] DateTime periodoReferencia)
        => Ok(await service.CalcularParaPeriodoAsync(unidadeId, periodoReferencia));

    [HttpGet("unidade/{unidadeId:int}")]
    public async Task<ActionResult<IEnumerable<RoyaltyResponseDto>>> GetPorUnidade(int unidadeId) =>
        Ok(await service.ListarPorUnidadeAsync(unidadeId));

    [HttpPut("{id:int}/pagamento")]
    public async Task<IActionResult> RegistrarPagamento(int id, RegistrarPagamentoDto dto)
    {
        await service.RegistrarPagamentoAsync(id, dto);
        return NoContent();
    }
}