namespace Gestao_Franquias.Api.Models;

public class ItemVenda
{
    public int Id { get; set; }
    public int Quantidade { get; init; }
    public decimal PrecoUnitario { get; init; }
    public decimal Subtotal => Quantidade * PrecoUnitario; // calculado, não mapeado no banco

    public int VendaId { get; init; }
    public Venda Venda { get; set; } = null!;

    public int ProdutoServicoId { get; init; }
    public ProdutoServico ProdutoServico { get; set; } = null!;
}