namespace Gestao_Franquias.Api.DTOs.Auth;

public class TokenResponseDto
{
    public string Token { get; set; } = null!;
    public int ExpiraEmMinutos { get; set; }
    public string Nome { get; set; } = null!;
    public string Perfil { get; set; } = null!;
}