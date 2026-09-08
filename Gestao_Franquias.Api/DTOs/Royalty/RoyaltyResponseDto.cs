namespace Gestao_Franquias.Api.DTOs.Royalty;

public class RoyaltyResponseDto
{
    public int Id { get; set; }
    public DateTime PeriodoReferencia { get; set; }
    public decimal PercentualAplicado { get; set; }
    public decimal FaturamentoBase { get; set; }
    public decimal ValorCalculado { get; set; }
    public string StatusPagamento { get; set; } = null!;
    public DateTime? DataPagamento { get; set; }
}