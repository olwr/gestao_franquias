using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Franqueadora;

public class FranqueadoraCreateDto
{
    [Required, MaxLength(200)] public string RazaoSocial { get; set; } = null!;
    [Required, StringLength(14, MinimumLength = 14)] public string Cnpj { get; set; } = null!;
    public string? Telefone { get; set; }
    [EmailAddress] public string? Email { get; set; }
}