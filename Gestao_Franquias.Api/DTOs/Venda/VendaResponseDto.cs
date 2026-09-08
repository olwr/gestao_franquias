namespace Gestao_Franquias.Api.DTOs.Venda;

public class VendaResponseDto
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public decimal ValorTotal { get; set; }
    public List<ItemVendaResponseDto> Itens { get; set; } = new();
}