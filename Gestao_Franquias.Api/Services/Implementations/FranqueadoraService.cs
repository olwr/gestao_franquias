using AutoMapper;
using Gestao_Franquias.Api.DTOs.Franqueadora;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class FranqueadoraService(IFranqueadoraRepository repo, IMapper mapper) : IFranqueadoraService
{
    public async Task<IEnumerable<FranqueadoraResponseDto>> GetAllAsync()
    {
        var franqueadoras = await repo.GetAllAsync();
        return franqueadoras.Select(f => mapper.Map<FranqueadoraResponseDto>(f));
    }

    public async Task<FranqueadoraResponseDto?> ObterPorIdAsync(int id)
    {
        var franqueadora = await repo.GetByIdAsync(id);
        return franqueadora is null ? null : mapper.Map<FranqueadoraResponseDto>(franqueadora);
    }

    public async Task<FranqueadoraResponseDto> CriarAsync(FranqueadoraCreateDto dto)
    {
        var existente = await repo.GetByCnpjAsync(dto.Cnpj);
        if (existente is not null)
            throw new BusinessException("Já existe uma franqueadora cadastrada com este CNPJ.");

        var franqueadora = mapper.Map<Franqueadora>(dto);
        franqueadora.Ativo = true;

        await repo.AddAsync(franqueadora);
        await repo.SaveChangesAsync();
        return mapper.Map<FranqueadoraResponseDto>(franqueadora);
    }

    public async Task AtualizarAsync(int id, FranqueadoraUpdateDto dto)
    {
        var franqueadora = await repo.GetByIdAsync(id)
                           ?? throw new NotFoundException($"Franqueadora {id} não encontrada.");

        franqueadora.RazaoSocial = dto.RazaoSocial;
        franqueadora.Telefone = dto.Telefone;
        franqueadora.Email = dto.Email;

        repo.Update(franqueadora);
        await repo.SaveChangesAsync();
    }

    public async Task InativarAsync(int id)
    {
        var franqueadora = await repo.GetByIdAsync(id)
                           ?? throw new NotFoundException($"Franqueadora {id} não encontrada.");
        franqueadora.Ativo = false;
        repo.Update(franqueadora);
        await repo.SaveChangesAsync();
    }
}