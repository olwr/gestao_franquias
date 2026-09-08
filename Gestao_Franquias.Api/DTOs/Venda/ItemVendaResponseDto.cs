namespace Gestao_Franquias.Api.DTOs.Venda;

public class ItemVendaResponseDto
{
    public string ProdutoNome { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}