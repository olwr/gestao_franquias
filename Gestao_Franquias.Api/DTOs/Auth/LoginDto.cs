using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Auth;

public class LoginDto
{
    [Required, EmailAddress] public string Email { get; set; } = null!;
    [Required] public string Senha { get; set; } = null!;
}