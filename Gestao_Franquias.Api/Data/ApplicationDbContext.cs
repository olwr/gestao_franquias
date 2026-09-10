using Microsoft.EntityFrameworkCore;
using Gestao_Franquias.Api.Models;

namespace Gestao_Franquias.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Franqueadora> Franqueadoras => Set<Franqueadora>();
    public DbSet<UnidadeFranqueada> Unidades => Set<UnidadeFranqueada>();
    public DbSet<Franqueado> Franqueados => Set<Franqueado>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<ProdutoServico> Produtos => Set<ProdutoServico>();
    public DbSet<Estoque> Estoques => Set<Estoque>();
    public DbSet<MovimentacaoEstoque> Movimentacoes => Set<MovimentacaoEstoque>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<ItemVenda> ItensVenda => Set<ItemVenda>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Royalty> Royalties => Set<Royalty>();
    public DbSet<ChamadoSuporte> Chamados => Set<ChamadoSuporte>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UnidadeFranqueada>().HasIndex(u => u.Cnpj).IsUnique();
        modelBuilder.Entity<Fornecedor>().HasIndex(f => f.Cnpj).IsUnique();
        modelBuilder.Entity<Franqueadora>().HasIndex(f => f.Cnpj).IsUnique();

        modelBuilder.Entity<Estoque>()
            .HasIndex(e => new { e.UnidadeFranqueadaId, e.ProdutoServicoId })
            .IsUnique();

        modelBuilder.Entity<Venda>()
            .HasOne(v => v.UnidadeFranqueada)
            .WithMany(u => u.Vendas)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ItemVenda>()
            .HasOne(i => i.Venda)
            .WithMany(v => v.Itens)
            .OnDelete(DeleteBehavior.Cascade); // item pertence exclusivamente à venda

        modelBuilder.Entity<ProdutoServico>()
            .Property(p => p.PrecoBase).HasColumnType("numeric(12,2)");
        modelBuilder.Entity<ItemVenda>()
            .Property(i => i.PrecoUnitario).HasColumnType("numeric(12,2)");
        modelBuilder.Entity<Venda>()
            .Property(v => v.ValorTotal).HasColumnType("numeric(12,2)");
        modelBuilder.Entity<Royalty>()
            .Property(r => r.ValorCalculado).HasColumnType("numeric(12,2)");

        base.OnModelCreating(modelBuilder);
    }
}