namespace Gestao_Franquias.Api.DTOs.Chamado;

public class ChamadoResponseDto
{
    public int Id { get; set; }
    public string Categoria { get; set; } = null!;
    public string Prioridade { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public DateTime DataAbertura { get; set; }
    public DateTime? DataEncerramento { get; set; }
}