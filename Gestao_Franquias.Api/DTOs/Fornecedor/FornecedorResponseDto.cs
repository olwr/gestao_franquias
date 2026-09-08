namespace Gestao_Franquias.Api.DTOs.Fornecedor;

public class FornecedorResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string? Contato { get; set; }
    public bool Ativo { get; set; }
}