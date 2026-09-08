namespace Gestao_Franquias.Api.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string SenhaHash { get; set; } = null!;
    public Perfil Perfil { get; set; }
    public bool Ativo { get; set; } = true;

    // Nulo para Admnistrador; obrigatório para GestorUnidade/Operador
    public int? UnidadeFranqueadaId { get; set; }
    public UnidadeFranqueada? UnidadeFranqueada { get; set; }
    public ICollection<Venda> Vendas { get; set; } = new List<Venda>();
}