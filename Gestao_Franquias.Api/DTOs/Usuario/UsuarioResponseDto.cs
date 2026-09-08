namespace Gestao_Franquias.Api.DTOs.Usuario;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Perfil { get; set; } = null!;
    public bool Ativo { get; set; }
    public int? UnidadeFranqueadaId { get; set; }
}