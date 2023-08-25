﻿using System.Reflection;
using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuLivroDeReceitas.Infrastructure;


public static class Bootstrapper {

    public static void AddRepositorio(this IServiceCollection services, IConfiguration configuration) {
        AddFluentMigrator(services, configuration);

        AddContext(services, configuration);
        AddUnidadeDeTrabalho(services);
        AddRepositorios(services);
    }


    private static void AddContext(IServiceCollection services, IConfiguration configuration) {

        var versaoServidor = new MySqlServerVersion(new Version(8, 0, 26));
        var connectionString = configuration.GetConexaoCompleta();

        services.AddDbContext<MeuLivroDeReceitasContext>(
            dbContextOptions => dbContextOptions.UseMySql(connectionString, versaoServidor)
        );
        
    }
        

    private static void AddUnidadeDeTrabalho(IServiceCollection services) {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
    }

    private static void AddRepositorios(IServiceCollection services) {
        services.AddScoped<IUsuarioWriteOnlyRepositorio, UsuarioRepositorio>()
            .AddScoped<IUsuarioReadOnlyRepositorio, UsuarioRepositorio>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration) {
        services.AddFluentMigratorCore().ConfigureRunner(c => 
            c.AddMySql5().WithGlobalConnectionString(configuration.GetConexaoCompleta())
            .ScanIn(Assembly.Load("MeuLivroDeReceitas.Infrastructure")).For.All()
        );
    }

}