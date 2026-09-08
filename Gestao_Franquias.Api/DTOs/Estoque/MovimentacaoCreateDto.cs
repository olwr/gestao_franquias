namespace Gestao_Franquias.Api.DTOs.Estoque;

public class MovimentacaoCreateDto
{
    public int UnidadeFranqueadaId { get; set; }
    public int ProdutoServicoId { get; set; }
    public string Tipo { get; set; } = null!; // "Entrada" ou "Saida"
    public int Quantidade { get; set; }
    public string? Motivo { get; set; }
}