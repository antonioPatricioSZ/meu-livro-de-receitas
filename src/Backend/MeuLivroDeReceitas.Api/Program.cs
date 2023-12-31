using AutoMapper;
using HashidsNet;
using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Api.Filtros.Converter;
using MeuLivroDeReceitas.Api.Filtros.Swagger;
using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Api.Middleware;
using MeuLivroDeReceitas.Application;
using MeuLivroDeReceitas.Application.Servicos.Automapper;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Infrastructure;
using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(config => config.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();
// Agora posso pegar os dados que vem da request, o token por exemplo

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.OperationFilter<HashidsOperationFilter>();
    options.SwaggerDoc(
        "v1", new Microsoft.OpenApi.Models.OpenApiInfo { 
            Title = "Meu Livro de Receitas API", Version = "1.0" 
        }
    );
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header utilizando o Bearer scheme. Example: \"Authorization: Bearer {token}\""
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement { {
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
        }
    });
});


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltrosDasExceptions)));
// qualquer excecao que for lancada vai ser executada a classe FiltrosDasExceptions

// builder.Services.AddAutoMapper(typeof(AutoMapperConfiguracao));
// Usar isso se n�o quiser usar o HashIds

builder.Services.AddScoped(provider => new MapperConfiguration(config => {
    config.AddProfile(new AutoMapperConfiguracao(provider.GetService<IHashids>()));
}).CreateMapper());

builder.Services.AddScoped<IAuthorizationHandler, UsuarioLogadoHandler>();
builder.Services.AddAuthorization(options => {
    options.AddPolicy(
        "UsuarioLogado", policy => policy.Requirements.Add(new UsuarioLogadoRequirement())
    );
});
builder.Services.AddScoped<UsuarioAutenticadoAttribute>();


builder.Services.AddSignalR();
builder.Services.AddCors(options => {
    options.AddPolicy(
        name: "PermitirApiRequest",
        build => build.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
    );
});

builder.Services.AddHealthChecks().AddDbContextCheck<MeuLivroDeReceitasContext>();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions {
    AllowCachingResponses = false, // para n�o fazer cache e ele sempre verificar se t� tudo ok
    ResultStatusCodes = {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirApiRequest");

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

        // E ao inv�s de chamar connection string, n�s chamamos uma fun��o nossa. Ou seja, a 
        // gente vai implementar uma fun��o para ser chamada como se fosse uma fun��o 
        // dessa vari�vel Configuration.

        // Todo m�todo que voc� for fazer para chamar dessa forma de uma vari�vel digamos 
        // que n�o � sua que n�o foi voc� criou. E, no caso de Configuration Manager, � uma 
        // uma classe interna do dotnet. Ent�o a gente chama isso de extens�o, ok.

        // Como que seu m�todo? Como ele vai saber que isso aqui � uma classe de extens�o, 
        // ou seja. Voc� vai ter acesso a essa fun��o a partir de uma classe
        // que implementa esse IConfiguration, porque ele usa esse modificador this antes 
        // ok, se voc� tirar ou n�o utilizar o this, n�o vai funcionar. Esse this significa 
        // que esse m�todo � uma extens�o e essa vari�vel (configuration de IConfiguration)
        // aqui � exatamente a vari�vel que voc� est� usando para chamar essa fun��o.

        Database.CriarDatabase(conexao, nomeDatabase);

        app.MigrateBancoDeDados();
    }

}

#pragma warning disable CA1050, S3903, S1118
public partial class Program { }
#pragma warning restore CA1050, S3903, S1118



