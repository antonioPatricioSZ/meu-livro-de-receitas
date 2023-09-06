using Bogus;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaOsTestes.Requisicoes;

public class RequisicaoRegistrarReceitaBuilder {

    public static RequisicaoRegistrarReceitaJson Construir() {

        return new Faker<RequisicaoRegistrarReceitaJson>()
            .RuleFor(c => c.Titulo, f => f.Commerce.ProductName())
            .RuleFor(c => c.Categoria, f => f.Random.Enum<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph());
    }

}
