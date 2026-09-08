using AutoMapper;
using Gestao_Franquias.Api.DTOs.Produto;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class ProdutoService(IProdutoRepository repo, IMapper mapper) : IProdutoService
{
    public async Task<IEnumerable<ProdutoResponseDto>> BuscarAsync(string? nome, int? categoriaId, bool? ativo)
    {
        var produtos = await repo.BuscarAsync(nome, categoriaId, ativo);
        return produtos.Select(p => mapper.Map<ProdutoResponseDto>(p));
    }

    public async Task<ProdutoResponseDto?> ObterPorIdAsync(int id)
    {
        var produto = await repo.GetComRelacionamentosAsync(id);
        return produto is null ? null : mapper.Map<ProdutoResponseDto>(produto);
    }

    public async Task<ProdutoResponseDto> CriarAsync(ProdutoCreateDto dto)
    {
        var produto = mapper.Map<ProdutoServico>(dto);
        produto.Ativo = true;
        await repo.AddAsync(produto);
        await repo.SaveChangesAsync();

        var criado = await repo.GetComRelacionamentosAsync(produto.Id);
        return mapper.Map<ProdutoResponseDto>(criado);
    }

    public async Task InativarAsync(int id)
    {
        var produto = await repo.GetByIdAsync(id)
                      ?? throw new NotFoundException($"Produto {id} não encontrado.");
        produto.Ativo = false;
        repo.Update(produto);
        await repo.SaveChangesAsync();
    }
}

public class CategoriaService(ICategoriaRepository repo, IMapper mapper) : ICategoriaService
{
    public async Task<IEnumerable<CategoriaResponseDto>> GetAllAsync()
    {
        var categorias = await repo.GetAllAsync();
        return categorias.Select(c => mapper.Map<CategoriaResponseDto>(c));
    }

    public async Task<CategoriaResponseDto> CriarAsync(CategoriaCreateDto dto)
    {
        var categoria = mapper.Map<Categoria>(dto);
        await repo.AddAsync(categoria);
        await repo.SaveChangesAsync();
        return mapper.Map<CategoriaResponseDto>(categoria);
    }
}