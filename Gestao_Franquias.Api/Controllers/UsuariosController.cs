using Gestao_Franquias.Api.DTOs.Usuario;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Roles = "Administrador")]
public class UsuariosController(IUsuarioService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
    {
        var usuario = await service.GetByIdAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<UsuarioResponseDto>> Create(UsuarioCreateDto dto)
    {
        var criado = await service.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Inativar(int id)
    {
        await service.InativarAsync(id);
        return NoContent();
    }
}