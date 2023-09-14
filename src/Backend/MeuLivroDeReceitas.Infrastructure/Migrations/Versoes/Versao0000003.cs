using FluentMigrator;

namespace MeuLivroDeReceitas.Infrastructure.Migrations.Versoes;

[Migration((long)NumeroVersoes.AlterarTabelaReceitas, "Adicionando coluna Tempo para o preparo")]
public class Versao0000003 : Migration {

    public override void Down() 
    {}

    public override void Up() {

        Alter.Table("receitas").AddColumn("TempoPreparo")
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0);
        // se ja tiver dados ele cria a coluna com o valor de 0
    }

}
