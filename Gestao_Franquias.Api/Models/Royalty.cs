namespace Gestao_Franquias.Api.Models;

public enum StatusPagamentoRoyalty
{
    Pendente = 1,
    Pago = 2,
    Atrasado = 3
}

public class Royalty
{
    public int Id { get; set; }

    // Primeiro dia do mês de referência (ex.: 2026-09-01 representa "setembro/2026")
    public DateTime PeriodoReferencia { get; init; }

    public decimal PercentualAplicado { get; init; }
    public decimal FaturamentoBase { get; init; }
    public decimal ValorCalculado { get; init; }
    public StatusPagamentoRoyalty StatusPagamento { get; set; } = StatusPagamentoRoyalty.Pendente;
    public DateTime? DataPagamento { get; set; }

    public int UnidadeFranqueadaId { get; init; }
    public UnidadeFranqueada UnidadeFranqueada { get; set; } = null!;
}