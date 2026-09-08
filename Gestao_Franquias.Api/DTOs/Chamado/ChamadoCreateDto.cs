using System.ComponentModel.DataAnnotations;
using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.DTOs.Chamado;

public class ChamadoCreateDto
{
    [Required] public int UnidadeFranqueadaId { get; set; }
    [Required] public CategoriaChamado Categoria { get; set; }
    [Required] public PrioridadeChamado Prioridade { get; set; }
    [Required, MaxLength(500)] public string Descricao { get; set; } = null!;
}