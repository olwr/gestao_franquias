namespace Gestao_Franquias.Api.Models;

public class Venda
{
    public int Id { get; set; }
    public DateTime DataHora { get; init; } = DateTime.UtcNow;
    public decimal ValorTotal { get; init; } // sempre calculado no servidor, nunca recebido do cliente

    public int UnidadeFranqueadaId { get; init; }
    public UnidadeFranqueada UnidadeFranqueada { get; set; } = null!;

    public int UsuarioId { get; init; }
    public Usuario Usuario { get; set; } = null!;

    public ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
}