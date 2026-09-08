using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Produto;

public class CategoriaCreateDto
{
    [Required, MaxLength(100)] public string Nome { get; set; } = null!;
}