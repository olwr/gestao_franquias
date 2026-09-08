using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Unidade;

public class UnidadeUpdateDto
{
    [Required, MaxLength(200)] public string Nome { get; set; } = null!;
    [Required] public string Endereco { get; set; } = null!;
    [Required] public string Cidade { get; set; } = null!;
    [Range(0, 100)] public decimal PercentualRoyalty { get; set; }
}