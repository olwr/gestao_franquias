namespace Gestao_Franquias.Api.Models;

public class Franqueadora
{
    public int Id { get; set; }
    public string RazaoSocial { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }

    public ICollection<UnidadeFranqueada> Unidades { get; set; } = new List<UnidadeFranqueada>();
}