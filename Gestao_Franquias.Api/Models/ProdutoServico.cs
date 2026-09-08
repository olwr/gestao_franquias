namespace Gestao_Franquias.Api.Models;

public class ProdutoServico
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public bool Ativo { get; set; } = true;

    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    // Opcional: nem todo produto precisa ter fornecedor cadastrado
    public int? FornecedorId { get; set; }
    public Fornecedor? Fornecedor { get; set; }

    public ICollection<Estoque> Estoques { get; set; } = new List<Estoque>();
    public ICollection<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
}