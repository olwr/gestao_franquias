using Gestao_Franquias.Api.DTOs.Venda;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/vendas")]
[Authorize]
public class VendasController : ControllerBase
{
    private readonly IVendaService _service;
    public VendasController(IVendaService service) => _service = service;

    [HttpPost]
    [Authorize(Roles = "Administrador,GestorUnidade,Operador")]
    public async Task<ActionResult<VendaResponseDto>> Create(VendaCreateDto dto) =>
        Ok(await _service.CriarAsync(dto));

    [HttpGet("unidade/{unidadeId:int}")]
    public async Task<ActionResult<IEnumerable<VendaResponseDto>>> GetPorUnidade(
        int unidadeId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
        => Ok(await _service.ListarPorUnidadeAsync(unidadeId, inicio, fim));
}