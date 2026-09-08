namespace Gestao_Franquias.Api.DTOs.Produto;

public class ProdutoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public bool Ativo { get; set; }
    public string CategoriaNome { get; set; } = null!;
    public string? FornecedorNome { get; set; }
}