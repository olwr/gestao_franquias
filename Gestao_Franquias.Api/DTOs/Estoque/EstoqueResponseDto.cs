namespace Gestao_Franquias.Api.DTOs.Estoque;

public class EstoqueResponseDto
{
    public int Id { get; set; }
    public string ProdutoNome { get; set; } = null!;
    public string UnidadeNome { get; set; } = null!;
    public int SaldoAtual { get; set; }
    public int EstoqueMinimo { get; set; }
}