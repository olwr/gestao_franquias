using Gestao_Franquias.Api.DTOs.Usuario;
using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface IUsuarioService
{
    Task<Usuario?> AutenticarAsync(string email, string senha);
    Task<IEnumerable<UsuarioResponseDto>> GetAllAsync();
    Task<UsuarioResponseDto?> GetByIdAsync(int id);
    Task<UsuarioResponseDto> CriarAsync(UsuarioCreateDto dto);
    Task InativarAsync(int id);
}