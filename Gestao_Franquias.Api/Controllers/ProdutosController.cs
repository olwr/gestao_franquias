using Gestao_Franquias.Api.DTOs.Produto;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/produtos")]
[Authorize]
public class ProdutosController(IProdutoService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetAll(
        [FromQuery] string? nome, [FromQuery] int? categoriaId, [FromQuery] bool? ativo)
        => Ok(await service.BuscarAsync(nome, categoriaId, ativo));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> GetById(int id)
    {
        var produto = await service.ObterPorIdAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ProdutoResponseDto>> Create(ProdutoCreateDto dto)
    {
        var criado = await service.CriarAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Inativar(int id)
    {
        await service.InativarAsync(id);
        return NoContent();
    }
}

[ApiController]
[Route("api/categorias")]
[Authorize]
public class CategoriasController(ICategoriaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponseDto>>> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<CategoriaResponseDto>> Create(CategoriaCreateDto dto) =>
        Ok(await service.CriarAsync(dto));
}