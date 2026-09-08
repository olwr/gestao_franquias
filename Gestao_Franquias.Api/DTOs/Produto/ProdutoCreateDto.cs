using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Produto;

public class ProdutoCreateDto
{
    [Required, MaxLength(150)] public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    [Range(0.01, double.MaxValue)] public decimal PrecoBase { get; set; }
    [Required] public int CategoriaId { get; set; }
    public int? FornecedorId { get; set; }
}