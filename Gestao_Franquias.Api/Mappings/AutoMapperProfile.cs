using Gestao_Franquias.Api.DTOs.Chamado;
using Gestao_Franquias.Api.DTOs.Estoque;
using Gestao_Franquias.Api.DTOs.Fornecedor;
using Gestao_Franquias.Api.DTOs.Produto;
using Gestao_Franquias.Api.DTOs.Royalty;
using Gestao_Franquias.Api.DTOs.Unidade;
using Gestao_Franquias.Api.DTOs.Usuario;
using Gestao_Franquias.Api.DTOs.Venda;
using Gestao_Franquias.Api.Models;
using AutoMapper;

namespace Gestao_Franquias.Api.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Usuário
        CreateMap<Usuario, UsuarioResponseDto>();

        // Unidade
        CreateMap<UnidadeFranqueada, UnidadeResponseDto>();
        CreateMap<UnidadeCreateDto, UnidadeFranqueada>();

        // Categoria / Produto
        CreateMap<Categoria, CategoriaResponseDto>();
        CreateMap<CategoriaCreateDto, Categoria>();
        CreateMap<ProdutoServico, ProdutoResponseDto>()
            .ForMember(d => d.CategoriaNome, o => o.MapFrom(s => s.Categoria.Nome))
            .ForMember(d => d.FornecedorNome, o => o.MapFrom(s => s.Fornecedor != null ? s.Fornecedor.Nome : null));
        CreateMap<ProdutoCreateDto, ProdutoServico>();

        // Fornecedor
        CreateMap<Fornecedor, FornecedorResponseDto>();
        CreateMap<FornecedorCreateDto, Fornecedor>();

        // Estoque
        CreateMap<Estoque, EstoqueResponseDto>()
            .ForMember(d => d.ProdutoNome, o => o.MapFrom(s => s.ProdutoServico.Nome))
            .ForMember(d => d.UnidadeNome, o => o.MapFrom(s => s.UnidadeFranqueada.Nome));
        CreateMap<MovimentacaoEstoque, MovimentacaoResponseDto>();

        // Venda
        CreateMap<Venda, VendaResponseDto>();
        CreateMap<ItemVenda, ItemVendaResponseDto>()
            .ForMember(d => d.ProdutoNome, o => o.MapFrom(s => s.ProdutoServico.Nome));

        // Royalty
        CreateMap<Royalty, RoyaltyResponseDto>();

        // Chamado
        CreateMap<ChamadoSuporte, ChamadoResponseDto>();
        CreateMap<ChamadoCreateDto, ChamadoSuporte>();
    }
}