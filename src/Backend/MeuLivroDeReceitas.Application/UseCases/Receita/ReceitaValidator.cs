using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Receita;
public class ReceitaValidator : AbstractValidator<RequisicaoReceitaJson> {

    public ReceitaValidator() {

        RuleFor(requisicao => requisicao.Titulo).NotEmpty();
        RuleFor(requisicao => requisicao.Categoria).IsInEnum();
        RuleFor(requisicao => requisicao.ModoPreparo).NotEmpty();
        RuleFor(requisicao => requisicao.Ingredientes).NotEmpty();

        RuleForEach(requisicao => requisicao.Ingredientes).ChildRules(ingrediente =>
        {
            ingrediente.RuleFor(requisicao => requisicao.Produto).NotEmpty();
            ingrediente.RuleFor(requisicao => requisicao.Quantidade).NotEmpty();
        });

        RuleFor(requisicao => requisicao.Ingredientes).Custom((ingredientes, context) =>
        {
            var produtosDistintos = ingredientes.Select(ingrediente => ingrediente.Produto).Distinct();
            if (produtosDistintos.Count() != ingredientes.Count)
            {
                context.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredientes", ""));
            }
        });
    }

}
