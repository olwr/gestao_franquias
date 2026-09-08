namespace Gestao_Franquias.Api.Models;

public enum TipoMovimentacao
{
    Entrada = 1,
    Saida = 2
}

public class MovimentacaoEstoque
{
    public int Id { get; set; }
    public TipoMovimentacao Tipo { get; init; }
    public int Quantidade { get; init; }
    public DateTime DataHora { get; init; } = DateTime.UtcNow;
    public string? Motivo { get; init; }

    public int EstoqueId { get; init; }
    public Estoque Estoque { get; set; } = null!;
}