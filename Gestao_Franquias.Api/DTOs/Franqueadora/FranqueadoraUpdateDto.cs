using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Franqueadora;

public class FranqueadoraUpdateDto
{
    [Required, MaxLength(200)] public string RazaoSocial { get; set; } = null!;
    public string? Telefone { get; set; }
    [EmailAddress] public string? Email { get; set; }
}