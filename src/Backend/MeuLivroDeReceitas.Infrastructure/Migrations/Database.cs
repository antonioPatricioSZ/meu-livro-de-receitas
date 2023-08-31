using Dapper;
using MySqlConnector;

namespace MeuLivroDeReceitas.Infrastructure.Migrations;
public static class Database {

    public static void CriarDatabase(
        string conexaoComBancoDeDados, 
        string nomeDatabase
    ){
        using var minhaConexao = new MySqlConnection(conexaoComBancoDeDados);
        // o using significa que antes da funcao terminar vai fechar a conexao
        // como banco de dados, liberando a memoria da conexao

        var parametros = new DynamicParameters();
        parametros.Add("nome", nomeDatabase);

        var registros = minhaConexao.Query(
            "SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @nome",
            parametros
        );

        if(!registros.Any() ) {
            minhaConexao.Execute($"CREATE DATABASE {nomeDatabase}");
        }
    }

}
