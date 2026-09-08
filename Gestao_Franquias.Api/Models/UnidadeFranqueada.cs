namespace Gestao_Franquias.Api.Models;

public class UnidadeFranqueada
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string Endereco { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public DateTime DataInicio { get; init; } // marco histórico: unidade não "muda" a data de abertura
    public bool Ativo { get; set; } = true;

    // Percentual de royalty aplicado a esta unidade (ex.: 5.00 = 5%)
    public decimal PercentualRoyalty { get; set; }

    public int FranqueadoraId { get; set; }
    public Franqueadora Franqueadora { get; set; } = null!;

    public ICollection<Franqueado> Franqueados { get; set; } = new List<Franqueado>();
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<Estoque> Estoques { get; set; } = new List<Estoque>();
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    public ICollection<Royalty> Royalties { get; set; } = new List<Royalty>();
    public ICollection<ChamadoSuporte> Chamados { get; set; } = new List<ChamadoSuporte>();
}