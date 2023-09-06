using AutoMapper;
using HashidsNet;
using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Api.Filtros.Swagger;
using MeuLivroDeReceitas.Api.Middleware;
using MeuLivroDeReceitas.Application;
using MeuLivroDeReceitas.Application.Servicos.Automapper;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Infrastructure;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(config => config.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();
// Agora posso pegar os dados que vem da request, o token por exemplo

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.OperationFilter<HashidsOperationFilter>();
});


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltrosDasExceptions)));
// qualquer excecao que for lancada vai ser executada a classe FiltrosDasExceptions

// builder.Services.AddAutoMapper(typeof(AutoMapperConfiguracao));
// Usar isso se não quiser usar o HashIds

builder.Services.AddScoped(provider => new MapperConfiguration(config => {
    config.AddProfile(new AutoMapperConfiguracao(provider.GetService<IHashids>()));
}).CreateMapper());


builder.Services.AddScoped<UsuarioAutenticadoAttribute>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AtualizarBaseDeDados();

app.UseMiddleware<CultureMiddleware>();

app.Run();


void AtualizarBaseDeDados() {

    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    using var context = serviceScope.ServiceProvider.GetService<MeuLivroDeReceitasContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals(
        "Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value) {
        var conexao = builder.Configuration.GetConexao();
        var nomeDatabase = builder.Configuration.GetNomeDatabase();

        // E ao invés de chamar connection string, nós chamamos uma função nossa. Ou seja, a 
        // gente vai implementar uma função para ser chamada como se fosse uma função 
        // dessa variável Configuration.

        // Todo método que você for fazer para chamar dessa forma de uma variável digamos 
        // que não é sua que não foi você criou. E, no caso de Configuration Manager, é uma 
        // uma classe interna do dotnet. Então a gente chama isso de extensão, ok.

        // Como que seu método? Como ele vai saber que isso aqui é uma classe de extensão, 
        // ou seja. Você vai ter acesso a essa função a partir de uma classe
        // que implementa esse IConfiguration, porque ele usa esse modificador this antes 
        // ok, se você tirar ou não utilizar o this, não vai funcionar. Esse this significa 
        // que esse método é uma extensão e essa variável (configuration de IConfiguration)
        // aqui é exatamente a variável que você está usando para chamar essa função.

        Database.CriarDatabase(conexao, nomeDatabase);

        app.MigrateBancoDeDados();
    }

}

#pragma warning disable CA1050, S3903, S1118
public partial class Program { }
#pragma warning restore CA1050, S3903, S1118