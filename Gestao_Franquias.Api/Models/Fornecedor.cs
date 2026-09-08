namespace Gestao_Franquias.Api.Models;

public class Fornecedor
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string? Contato { get; set; }
    public bool Ativo { get; set; } = true;

    public ICollection<ProdutoServico> Produtos { get; set; } = new List<ProdutoServico>();
}