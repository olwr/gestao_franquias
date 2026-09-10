using Gestao_Franquias.Api.DTOs.Franqueadora;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/franqueadoras")]
[Authorize]
public class FranqueadorasController(IFranqueadoraService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FranqueadoraResponseDto>>> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FranqueadoraResponseDto>> GetById(int id)
    {
        var franqueadora = await service.ObterPorIdAsync(id);
        return franqueadora is null ? NotFound() : Ok(franqueadora);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<FranqueadoraResponseDto>> Create(FranqueadoraCreateDto dto)
    {
        var criada = await service.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criada.Id }, criada);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(int id, FranqueadoraUpdateDto dto)
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