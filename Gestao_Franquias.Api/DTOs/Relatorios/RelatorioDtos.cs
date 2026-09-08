namespace Gestao_Franquias.Api.DTOs.Relatorios;

public class FaturamentoPorUnidadeDto
{
    public int UnidadeId { get; set; }
    public string UnidadeNome { get; set; } = null!;
    public decimal Total { get; set; }
}

public class ProdutoMaisVendidoDto
{
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; } = null!;
    public int QuantidadeTotal { get; set; }
}

public class ChamadosPorStatusDto
{
    public string Status { get; set; } = null!;
    public int Total { get; set; }
}