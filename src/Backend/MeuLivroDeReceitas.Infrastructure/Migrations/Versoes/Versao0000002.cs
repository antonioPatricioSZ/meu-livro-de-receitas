﻿using FluentMigrator;

namespace MeuLivroDeReceitas.Infrastructure.Migrations.Versoes;

[Migration((long)NumeroVersoes.CriarTabelaReceitas, "Cria tabela receitas")]

public class Versao0000002 : Migration {

    public override void Down() {}


    public override void Up() {
        CriarTabelaReceitas();
        CriarTabelaIngredientes();
    }


    private void CriarTabelaReceitas() {
        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Receitas"));

        tabela
            .WithColumn("Titulo").AsString(120).NotNullable()
            .WithColumn("Categoria").AsInt16().NotNullable()
            .WithColumn("ModoPreparo").AsString(5000).NotNullable();
    }

    private void CriarTabelaIngredientes() {
        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Ingredientes"));

        tabela
            .WithColumn("Produto").AsString(120).NotNullable()
            .WithColumn("Quantidade").AsString().NotNullable()
            .WithColumn("ReceitaId").AsInt64().NotNullable()
                .ForeignKey("FK_Ingrediente_Receita_Id", "Receitas", "Id");
    }

}