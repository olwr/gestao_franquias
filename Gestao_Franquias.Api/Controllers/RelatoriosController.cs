using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.DTOs.Relatorios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/relatorios")]
[Authorize]
public class RelatoriosController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("faturamento")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<IEnumerable<FaturamentoPorUnidadeDto>>> Faturamento(
        [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var query = context.Vendas.AsQueryable();
        if (inicio.HasValue) query = query.Where(v => v.DataHora >= inicio.Value);
        if (fim.HasValue) query = query.Where(v => v.DataHora <= fim.Value);

        var resultado = await query
            .GroupBy(v => new { v.UnidadeFranqueadaId, v.UnidadeFranqueada.Nome })
            .Select(g => new FaturamentoPorUnidadeDto
            {
                UnidadeId = g.Key.UnidadeFranqueadaId,
                UnidadeNome = g.Key.Nome,
                Total = g.Sum(v => v.ValorTotal)
            })
            .OrderByDescending(x => x.Total)
            .ToListAsync();

        return Ok(resultado);
    }

    [HttpGet("produtos-mais-vendidos")]
    public async Task<ActionResult<IEnumerable<ProdutoMaisVendidoDto>>> ProdutosMaisVendidos(
        [FromQuery] int top = 10)
    {
        var resultado = await context.ItensVenda
            .GroupBy(i => new { i.ProdutoServicoId, i.ProdutoServico.Nome })
            .Select(g => new ProdutoMaisVendidoDto
            {
                ProdutoId = g.Key.ProdutoServicoId,
                ProdutoNome = g.Key.Nome,
                QuantidadeTotal = g.Sum(i => i.Quantidade)
            })
            .OrderByDescending(x => x.QuantidadeTotal)
            .Take(top)
            .ToListAsync();

        return Ok(resultado);
    }

    [HttpGet("chamados-por-status")]
    public async Task<ActionResult<IEnumerable<ChamadosPorStatusDto>>> ChamadosPorStatus()
    {
        var resultado = await context.Chamados
            .GroupBy(c => c.Status)
            .Select(g => new ChamadosPorStatusDto { Status = g.Key.ToString(), Total = g.Count() })
            .ToListAsync();

        return Ok(resultado);
    }
}