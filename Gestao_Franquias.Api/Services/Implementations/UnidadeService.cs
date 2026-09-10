using AutoMapper;
using Gestao_Franquias.Api.DTOs.Unidade;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class UnidadeService(IUnidadeRepository repo, IMapper mapper) : IUnidadeService
{
    public async Task<IEnumerable<UnidadeResponseDto>> BuscarAsync(
        string? nome, string? cidade, bool? ativo, int page, int pageSize)
    {
        var unidades = await repo.BuscarAsync(nome, cidade, ativo, page, pageSize);
        return unidades.Select(mapper.Map<UnidadeResponseDto>);
    }

    public async Task<UnidadeResponseDto?> ObterPorIdAsync(int id)
    {
        var unidade = await repo.GetByIdAsync(id);
        return unidade is null ? null : mapper.Map<UnidadeResponseDto>(unidade);
    }

    public async Task<UnidadeResponseDto> CriarAsync(UnidadeCreateDto dto)
    {
        var existente = await repo.GetByCnpjAsync(dto.Cnpj);
        if (existente is not null)
            throw new BusinessException("Já existe uma unidade cadastrada com este CNPJ.");

        var unidade = mapper.Map<Models.UnidadeFranqueada>(dto);
        unidade.Ativo = true;

        await repo.AddAsync(unidade);
        await repo.SaveChangesAsync();
        return mapper.Map<UnidadeResponseDto>(unidade);
    }

    public async Task AtualizarAsync(int id, UnidadeUpdateDto dto)
    {
        var unidade = await repo.GetByIdAsync(id)
                      ?? throw new NotFoundException($"Unidade {id} não encontrada.");

        unidade.Nome = dto.Nome;
        unidade.Endereco = dto.Endereco;
        unidade.Cidade = dto.Cidade;
        unidade.PercentualRoyalty = dto.PercentualRoyalty;

        repo.Update(unidade);
        await repo.SaveChangesAsync();
    }

    public async Task InativarAsync(int id)
    {
        var unidade = await repo.GetByIdAsync(id)
                      ?? throw new NotFoundException($"Unidade {id} não encontrada.");
        unidade.Ativo = false;
        repo.Update(unidade);
        await repo.SaveChangesAsync();
    }
}