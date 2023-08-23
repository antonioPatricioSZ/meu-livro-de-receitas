using System.Reflection;
using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infrastructure;


public static class Bootstrapper {

    public static void AddRepositorio(this IServiceCollection services, IConfiguration configuration) {
        AddFluentMigrator(services, configuration);
    }

    public static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration) {
        services.AddFluentMigratorCore().ConfigureRunner(c => 
            c.AddMySql5().WithGlobalConnectionString(configuration.GetConexaoCompleta())
            .ScanIn(Assembly.Load("MeuLivroDeReceitas.Infrastructure")).For.All()
        );
    }

}
