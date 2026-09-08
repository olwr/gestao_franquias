using System.ComponentModel.DataAnnotations;
using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.DTOs.Usuario;

public class UsuarioCreateDto
{
    [Required, MaxLength(150)] public string Nome { get; set; } = null!;
    [Required, EmailAddress] public string Email { get; set; } = null!;
    [Required, MinLength(6)] public string Senha { get; set; } = null!;
    [Required] public Perfil Perfil { get; set; }
    public int? UnidadeFranqueadaId { get; set; }
}