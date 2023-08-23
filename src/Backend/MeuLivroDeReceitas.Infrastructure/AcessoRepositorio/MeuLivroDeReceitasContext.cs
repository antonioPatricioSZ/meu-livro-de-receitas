using MeuLivroDeReceitas.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;


public class MeuLivroDeReceitasContext : DbContext {

    public MeuLivroDeReceitasContext(
        DbContextOptions<MeuLivroDeReceitasContext> options
    ) : base(options) {}
    // o base é o proprio DbContext

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuLivroDeReceitasContext).Assembly);
    }
    // isso aplica as configuracoes que foram definidas em MeuLivroDeReceitasContext

}
