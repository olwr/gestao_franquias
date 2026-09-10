using AutoMapper;
using Gestao_Franquias.Api.DTOs.Fornecedor;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class FornecedorService(IFornecedorRepository repo, IMapper mapper) : IFornecedorService
{
    public async Task<IEnumerable<FornecedorResponseDto>> GetAllAsync()
    {
        var fornecedores = await repo.GetAllAsync();
        return fornecedores.Select(mapper.Map<FornecedorResponseDto>);
    }

    public async Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto)
    {
        var existente = await repo.GetByCnpjAsync(dto.Cnpj);
        if (existente is not null)
            throw new BusinessException("Já existe um fornecedor cadastrado com este CNPJ.");

        var fornecedor = mapper.Map<Fornecedor>(dto);
        fornecedor.Ativo = true;
        await repo.AddAsync(fornecedor);
        await repo.SaveChangesAsync();
        return mapper.Map<FornecedorResponseDto>(fornecedor);
    }

    public async Task InativarAsync(int id)
    {
        var fornecedor = await repo.GetByIdAsync(id)
                         ?? throw new NotFoundException($"Fornecedor {id} não encontrado.");
        fornecedor.Ativo = false;
        repo.Update(fornecedor);
        await repo.SaveChangesAsync();
    }
}