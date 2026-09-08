using AutoMapper;
using Gestao_Franquias.Api.DTOs.Chamado;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class ChamadoService(IRepository<ChamadoSuporte> repo, IMapper mapper) : IChamadoService
{
    public async Task<ChamadoResponseDto> CriarAsync(ChamadoCreateDto dto)
    {
        var chamado = mapper.Map<ChamadoSuporte>(dto);
        await repo.AddAsync(chamado);
        await repo.SaveChangesAsync();
        return mapper.Map<ChamadoResponseDto>(chamado);
    }

    public async Task<IEnumerable<ChamadoResponseDto>> ListarPorUnidadeAsync(int unidadeId)
    {
        var chamados = await repo.FindAsync(c => c.UnidadeFranqueadaId == unidadeId);
        return chamados.OrderByDescending(c => c.DataAbertura)
            .Select(c => mapper.Map<ChamadoResponseDto>(c));
    }

    public async Task AtualizarStatusAsync(int id, AtualizarStatusDto dto)
    {
        var chamado = await repo.GetByIdAsync(id)
                      ?? throw new NotFoundException($"Chamado {id} não encontrado.");

        chamado.Status = dto.Status;
        if (dto.Status == StatusChamado.Encerrado)
            chamado.DataEncerramento = DateTime.UtcNow;

        repo.Update(chamado);
        await repo.SaveChangesAsync();
    }
}