using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Unidade;

public class UnidadeCreateDto
{
    [Required, MaxLength(200)] public string Nome { get; set; } = null!;

    [Required, StringLength(14, MinimumLength = 14)]
    public string Cnpj { get; set; } = null!;

    [Required] public string Endereco { get; set; } = null!;
    [Required] public string Cidade { get; set; } = null!;
    [Required] public DateTime DataInicio { get; set; }
    [Range(0, 100)] public decimal PercentualRoyalty { get; set; }
    [Required] public int FranqueadoraId { get; set; }
}