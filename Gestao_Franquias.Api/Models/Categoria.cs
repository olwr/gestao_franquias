namespace Gestao_Franquias.Api.Models;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;

    public ICollection<ProdutoServico> Produtos { get; set; } = new List<ProdutoServico>();
}