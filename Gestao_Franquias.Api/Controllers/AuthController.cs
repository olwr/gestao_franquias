using Gestao_Franquias.Api.DTOs.Auth;
using Gestao_Franquias.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gestao_Franquias.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUsuarioService usuarioService, ITokenService tokenService) : ControllerBase
{
    private readonly IUsuarioService _usuarioService = usuarioService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponseDto>> Login(LoginDto dto)
    {
        var usuario = await _usuarioService.AutenticarAsync(dto.Email, dto.Senha);
        if (usuario is null)
            return Unauthorized(new { error = "Credenciais inválidas." });

        var token = tokenService.GerarToken(usuario);
        return Ok(new TokenResponseDto
        {
            Token = token,
            ExpiraEmMinutos = 60,
            Nome = usuario.Nome,
            Perfil = usuario.Perfil.ToString()
        });
    }
}