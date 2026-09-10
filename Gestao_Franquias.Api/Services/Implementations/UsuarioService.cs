using AutoMapper;
using Gestao_Franquias.Api.DTOs.Usuario;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class UsuarioService(IUsuarioRepository repo, IMapper mapper) : IUsuarioService
{
    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        var usuario = await repo.GetByEmailAsync(email);
        if (usuario is null || !usuario.Ativo) return null;
        return BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash) ? usuario : null;
    }

    public async Task<IEnumerable<UsuarioResponseDto>> GetAllAsync()
    {
        var usuarios = await repo.GetAllAsync();
        return usuarios.Select(mapper.Map<UsuarioResponseDto>);
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
    {
        var usuario = await repo.GetByIdAsync(id);
        return usuario is null ? null : mapper.Map<UsuarioResponseDto>(usuario);
    }

    public async Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto)
    {
        var existente = await repo.GetByEmailAsync(dto.Email);
        if (existente is not null)
            throw new BusinessException("Já existe um usuário cadastrado com este e-mail.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Perfil = dto.Perfil,
            UnidadeFranqueadaId = dto.UnidadeFranqueadaId,
            Ativo = true
        };

        await repo.AddAsync(usuario);
        await repo.SaveChangesAsync();
        return mapper.Map<UsuarioResponseDto>(usuario);
    }

    public async Task InativarAsync(int id)
    {
        var usuario = await repo.GetByIdAsync(id)
                      ?? throw new NotFoundException($"Usuário {id} não encontrado.");
        usuario.Ativo = false;
        repo.Update(usuario);
        await repo.SaveChangesAsync();
    }
}