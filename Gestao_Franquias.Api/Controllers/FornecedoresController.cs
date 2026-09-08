using Gestao_Franquias.Api.DTOs.Fornecedor;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/fornecedores")]
[Authorize]
public class FornecedoresController(IFornecedorService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FornecedorResponseDto>>> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<FornecedorResponseDto>> Create(FornecedorCreateDto dto) =>
        Ok(await service.CriarAsync(dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Inativar(int id)
    {
        await service.InativarAsync(id);
        return NoContent();
    }
}