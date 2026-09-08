using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Fornecedor;

public class FornecedorCreateDto
{
    [Required, MaxLength(150)] public string Nome { get; set; } = null!;

    [Required, StringLength(14, MinimumLength = 14)]
    public string Cnpj { get; set; } = null!;

    public string? Contato { get; set; }
}