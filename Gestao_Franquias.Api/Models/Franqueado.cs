namespace Gestao_Franquias.Api.Models;

public class Franqueado
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public bool ResponsavelPrincipal { get; set; }

    public int UnidadeFranqueadaId { get; set; }
    public UnidadeFranqueada UnidadeFranqueada { get; set; } = null!;
}