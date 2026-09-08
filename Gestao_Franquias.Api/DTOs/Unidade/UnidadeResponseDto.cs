namespace Gestao_Franquias.Api.DTOs.Unidade;

public class UnidadeResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public bool Ativo { get; set; }
    public decimal PercentualRoyalty { get; set; }
    public DateTime DataInicio { get; set; }
}