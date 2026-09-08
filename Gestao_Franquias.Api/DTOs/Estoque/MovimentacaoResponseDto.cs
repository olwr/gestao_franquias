namespace Gestao_Franquias.Api.DTOs.Estoque;

public class MovimentacaoResponseDto
{
    public int Id { get; set; }
    public string Tipo { get; set; } = null!;
    public int Quantidade { get; set; }
    public DateTime DataHora { get; set; }
    public string? Motivo { get; set; }
}