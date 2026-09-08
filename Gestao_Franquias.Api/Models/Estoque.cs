namespace Gestao_Franquias.Api.Models;

public class Estoque
{
    public int Id { get; set; }
    public int SaldoAtual { get; set; }
    public int EstoqueMinimo { get; set; }

    public int UnidadeFranqueadaId { get; init; }
    public UnidadeFranqueada UnidadeFranqueada { get; set; } = null!;

    public int ProdutoServicoId { get; init; }
    public ProdutoServico ProdutoServico { get; set; } = null!;

    public ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();
}