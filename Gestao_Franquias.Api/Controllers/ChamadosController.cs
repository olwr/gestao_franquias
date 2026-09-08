using Gestao_Franquias.Api.DTOs.Chamado;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/chamados")]
[Authorize]
public class ChamadosController : ControllerBase
{
    private readonly IChamadoService _service;
    public ChamadosController(IChamadoService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<ChamadoResponseDto>> Create(ChamadoCreateDto dto) =>
        Ok(await _service.CriarAsync(dto));

    [HttpGet("unidade/{unidadeId:int}")]
    public async Task<ActionResult<IEnumerable<ChamadoResponseDto>>> GetPorUnidade(int unidadeId) =>
        Ok(await _service.ListarPorUnidadeAsync(unidadeId));

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> AtualizarStatus(int id, AtualizarStatusDto dto)
    {
        await _service.AtualizarStatusAsync(id, dto);
        return NoContent();
    }
}