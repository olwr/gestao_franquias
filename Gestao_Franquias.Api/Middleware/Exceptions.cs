namespace Gestao_Franquias.Api.Middleware;

public class BusinessException(string message) : Exception(message);

public class NotFoundException(string message) : Exception(message);

public class ForbiddenException(string message) : Exception(message);