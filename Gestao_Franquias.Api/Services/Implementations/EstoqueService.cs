using AutoMapper;
using Gestao_Franquias.Api.Data;
using Gestao_Franquias.Api.DTOs.Estoque;
using Gestao_Franquias.Api.Middleware;
using Gestao_Franquias.Api.Models;
using Gestao_Franquias.Api.Repositories.Interfaces;
using Gestao_Franquias.Api.Services.Interfaces;

namespace Gestao_Franquias.Api.Services.Implementations;

public class EstoqueService(IEstoqueRepository estoqueRepo, ApplicationDbContext context, IMapper mapper)
    : IEstoqueService
{
    public async Task<IEnumerable<EstoqueResponseDto>> ListarPorUnidadeAsync(int unidadeId)
    {
        var estoques = await estoqueRepo.ListarPorUnidadeAsync(unidadeId);
        return estoques.Select(mapper.Map<EstoqueResponseDto>);
    }

    public async Task<IEnumerable<EstoqueResponseDto>> ListarCriticosAsync()
    {
        var criticos = await estoqueRepo.ListarCriticosAsync();
        return criticos.Select(mapper.Map<EstoqueResponseDto>);
    }

    public async Task<MovimentacaoResponseDto> RegistrarMovimentacaoAsync(MovimentacaoCreateDto dto)
    {
        if (!Enum.TryParse<TipoMovimentacao>(dto.Tipo, ignoreCase: true, out var tipo))
            throw new BusinessException("Tipo de movimentação inválido. Use 'Entrada' ou 'Saida'.");

        if (dto.Quantidade <= 0)
            throw new BusinessException("Quantidade deve ser maior que zero.");

        var estoque = await estoqueRepo.ObterAsync(dto.UnidadeFranqueadaId, dto.ProdutoServicoId);
        var estoqueJaExistia = estoque is not null;

        // Cria o registro de estoque na primeira movimentação, se ainda não existir.
        estoque ??= new Estoque
        {
            UnidadeFranqueadaId = dto.UnidadeFranqueadaId,
            ProdutoServicoId = dto.ProdutoServicoId,
            SaldoAtual = 0,
            EstoqueMinimo = 0
        };

        if (!estoqueJaExistia)
            await estoqueRepo.AddAsync(estoque);

        if (tipo == TipoMovimentacao.Saida && estoque.SaldoAtual < dto.Quantidade)
            throw new BusinessException(
                $"Estoque insuficiente. Saldo atual: {estoque.SaldoAtual}, solicitado: {dto.Quantidade}.");

        estoque.SaldoAtual += tipo == TipoMovimentacao.Entrada ? dto.Quantidade : -dto.Quantidade;

        // Só chamamos Update() se o estoque já era rastreado como existente;
        // se acabou de ser criado (estado "Added"), chamar Update() o rebaixaria
        // para "Modified" e o INSERT nunca aconteceria.
        if (estoqueJaExistia)
            estoqueRepo.Update(estoque);

        // Associação via propriedade de navegação: o EF Core resolve o EstoqueId
        // automaticamente no momento do SaveChanges, mesmo o Estoque sendo novo
        // e ainda sem Id gerado neste ponto do código.
        var movimentacao = new MovimentacaoEstoque
        {
            Estoque = estoque,
            Tipo = tipo,
            Quantidade = dto.Quantidade,
            DataHora = DateTime.UtcNow,
            Motivo = dto.Motivo ?? tipo.ToString()
        };
        context.Movimentacoes.Add(movimentacao);

        await estoqueRepo.SaveChangesAsync();

        return mapper.Map<MovimentacaoResponseDto>(movimentacao);
    }
}