using System.Reflection;
using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infrastructure;


public static class Bootstrapper {

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        AddFluentMigrator(services, configuration);

        AddContext(services, configuration);
        AddUnidadeDeTrabalho(services);
        AddRepositorios(services);
    }


    private static void AddContext(IServiceCollection services, IConfiguration configuration) {

        _ = bool.TryParse(
            configuration.GetSection(
                "Configuracoes:BancoDeDadosInMemory"
            ).Value,
            out bool bancoDeDadosInMemory
        );

        if(!bancoDeDadosInMemory) {
            var versaoServidor = new MySqlServerVersion(new Version(8, 0, 26));
            var connectionString = configuration.GetConexaoCompleta();

            services.AddDbContext<MeuLivroDeReceitasContext>(
                dbContextOptions => dbContextOptions.UseMySql(connectionString, versaoServidor)
            );
        }
        
    }
        

    private static void AddUnidadeDeTrabalho(IServiceCollection services) {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
    }

    private static void AddRepositorios(IServiceCollection services) {
        services.AddScoped<IUsuarioWriteOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IUsuarioReadOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IUsuarioUpdateOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IReceitaWriteOnlyRepositorio, ReceitaRepositorio>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration) {
        _ = bool.TryParse(
           configuration.GetSection(
               "Configuracoes:BancoDeDadosInMemory"
           ).Value,
           out bool bancoDeDadosInMemory
        );

        if (!bancoDeDadosInMemory) {
            services.AddFluentMigratorCore()
                .ConfigureRunner(c =>
                   c.AddMySql5().WithGlobalConnectionString(configuration.GetConexaoCompleta())
                   .ScanIn(Assembly.Load("MeuLivroDeReceitas.Infrastructure")).For.All()
                );
        }
           
    }

}
