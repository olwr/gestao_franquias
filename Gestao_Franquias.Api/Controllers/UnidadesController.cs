using Gestao_Franquias.Api.DTOs.Unidade;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/unidades")]
[Authorize]
public class UnidadesController(IUnidadeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnidadeResponseDto>>> GetAll(
        [FromQuery] string? nome, [FromQuery] string? cidade, [FromQuery] bool? ativo,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await service.BuscarAsync(nome, cidade, ativo, page, pageSize));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UnidadeResponseDto>> GetById(int id)
    {
        var unidade = await service.ObterPorIdAsync(id);
        return unidade is null ? NotFound() : Ok(unidade);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<UnidadeResponseDto>> Create(UnidadeCreateDto dto)
    {
        var criado = await service.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(int id, UnidadeUpdateDto dto)
    {
        await service.AtualizarAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Inativar(int id)
    {
        await service.InativarAsync(id);
        return NoContent();
    }
}