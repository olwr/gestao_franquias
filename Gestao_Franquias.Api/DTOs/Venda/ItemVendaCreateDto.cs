using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Venda;

public class ItemVendaCreateDto
{
    [Required] public int ProdutoServicoId { get; set; }
    [Range(1, int.MaxValue)] public int Quantidade { get; set; }
}