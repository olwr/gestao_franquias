namespace Gestao_Franquias.Api.Models;

public enum CategoriaChamado
{
    Financeiro = 1,
    Operacional = 2,
    TI = 3,
    Suprimentos = 4
}

public enum PrioridadeChamado
{
    Baixa = 1,
    Media = 2,
    Alta = 3,
    Urgente = 4
}

public enum StatusChamado
{
    Aberto = 1,
    EmAndamento = 2,
    Resolvido = 3,
    Encerrado = 4
}

public class ChamadoSuporte
{
    public int Id { get; set; }
    public CategoriaChamado Categoria { get; init; }
    public string Descricao { get; init; } = null!;
    public DateTime DataAbertura { get; init; } = DateTime.UtcNow;

    public PrioridadeChamado Prioridade { get; set; } // pode ser reclassificada durante o atendimento
    public StatusChamado Status { get; set; } = StatusChamado.Aberto;
    public DateTime? DataEncerramento { get; set; }

    public int UnidadeFranqueadaId { get; init; }
    public UnidadeFranqueada UnidadeFranqueada { get; set; } = null!;
}