using Bogus;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaOsTestes.Requisicoes;

public class RequisicaoRegistrarReceitaBuilder {

    public static RequisicaoReceitaJson Construir() {

        return new Faker<RequisicaoReceitaJson>()
            .RuleFor(c => c.Titulo, f => f.Commerce.ProductName())
            .RuleFor(c => c.Categoria, f => f.Random.Enum<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph());
    }

}
