using System.ComponentModel.DataAnnotations;

namespace Gestao_Franquias.Api.DTOs.Venda;

public class VendaCreateDto
{
    [Required] public int UnidadeFranqueadaId { get; set; }
    [Required] public int UsuarioId { get; set; }

    [Required, MinLength(1, ErrorMessage = "A venda deve ter ao menos 1 item.")]
    public List<ItemVendaCreateDto> Itens { get; set; } = new();
}