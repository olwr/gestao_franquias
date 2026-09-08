using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Services.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}