using System.Runtime.Intrinsics.X86;
using Microsoft.Extensions.Configuration;

namespace MeuLivroDeReceitas.Domain.Extension;
public static class RepositorioExtension {

    public static string GetNomeDatabase(this IConfiguration configuration) {
        var nomeDatabase = configuration.GetConnectionString("NomeDatabase");
        // só vai funcionar pq tem um objeto chamado ConnectionString no appsettings.json
        // Isso aqui não é um parâmetro. Por isso usa o this para falar que é exatamente esse
        // valor aqui, é a variável que está sendo utilizada para chamar essa classe
        return nomeDatabase;
    }
    public static string GetConexao(this IConfiguration configuration) {
        var conexao = configuration.GetConnectionString("Conexao");

        return conexao;
    }

    public static string GetConexaoCompleta(this IConfiguration configuration) {
        var nomeDatabase = configuration.GetNomeDatabase();
        var conexao = configuration.GetConexao();

        return $"{conexao}Database={nomeDatabase}";
    }


}
