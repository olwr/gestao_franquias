namespace Gestao_Franquias.Api.DTOs.Franqueadora;

public class FranqueadoraResponseDto
{
    public int Id { get; set; }
    public string RazaoSocial { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public bool Ativo { get; set; }
}