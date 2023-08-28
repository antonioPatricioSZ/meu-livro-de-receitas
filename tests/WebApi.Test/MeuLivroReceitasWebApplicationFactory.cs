using MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;



public class MeuLivroReceitasWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.UseEnvironment("Test").ConfigureServices(services => {

            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(MeuLivroDeReceitasContext)
            );

            if(descriptor != null ) {
                services.Remove(descriptor);
            }

            var provider =  services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<MeuLivroDeReceitasContext>(options => {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(provider);
            });

            var serviceProvider = services.BuildServiceProvider();

            var scope = serviceProvider.CreateScope();
            var scopeService = scope.ServiceProvider;

            var database = scopeService.GetRequiredService<MeuLivroDeReceitasContext>();

            database.Database.EnsureDeleted();
        });
        // Vai executar em ambiente de teste
    }

}
