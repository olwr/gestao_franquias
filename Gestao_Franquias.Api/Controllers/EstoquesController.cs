using Gestao_Franquias.Api.DTOs.Estoque;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/estoques")]
[Authorize]
public class EstoquesController(IEstoqueService service) : ControllerBase
{
    [HttpGet("unidade/{unidadeId:int}")]
    public async Task<ActionResult<IEnumerable<EstoqueResponseDto>>> GetPorUnidade(int unidadeId) =>
        Ok(await service.ListarPorUnidadeAsync(unidadeId));

    [HttpGet("criticos")]
    public async Task<ActionResult<IEnumerable<EstoqueResponseDto>>> GetCriticos() =>
        Ok(await service.ListarCriticosAsync());

    [HttpPost("movimentacoes")]
    [Authorize(Roles = "Administrador,GestorUnidade,Operador")]
    public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarMovimentacao(MovimentacaoCreateDto dto) =>
        Ok(await service.RegistrarMovimentacaoAsync(dto));
}