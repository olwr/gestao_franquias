using System.ComponentModel.DataAnnotations;
using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.DTOs.Chamado;

public class AtualizarStatusDto
{
    [Required] public StatusChamado Status { get; set; }
}