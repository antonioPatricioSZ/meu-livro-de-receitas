using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita;
public class ReceitaValidator : AbstractValidator<RequisicaoReceitaJson> {

    public ReceitaValidator() {

        RuleFor(requisicao => requisicao.Titulo).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.TITULO_RECEITA_EMBRANCO);

        RuleFor(requisicao => requisicao.Categoria).IsInEnum()
            .WithMessage(ResourceMensagensDeErro.CATEGORIA_RECEITA_INVALIDA);

        RuleFor(requisicao => requisicao.ModoPreparo).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.MODOPREPARO_RECEITA_EMBRANCO);

        RuleFor(requisicao => requisicao.Ingredientes).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.RECEITA_MINIMO_UM_INGREDIENTE);

        RuleFor(requisicao => requisicao.TempoPreparo).InclusiveBetween(1, 1000)
            .WithMessage(ResourceMensagensDeErro.TEMPO_PREPARO_INVALIDO);

        RuleForEach(requisicao => requisicao.Ingredientes).ChildRules(ingrediente => {
            ingrediente.RuleFor(requisicao => requisicao.Produto).NotEmpty()
                .WithMessage(ResourceMensagensDeErro.RECEITA_INGREDIENTE_PRODUTO_EMBRANCO);
            ingrediente.RuleFor(requisicao => requisicao.Quantidade).NotEmpty()
                .WithMessage(ResourceMensagensDeErro.RECEITA_INGREDIENTE_QUANTIDADE_EMBRANCO);
        });

        RuleFor(requisicao => requisicao.Ingredientes).Custom((ingredientes, context) => {
            var produtosDistintos = ingredientes.Select(
                ingrediente => ingrediente.Produto.RemoverAcentos().ToLower()
            ).Distinct();

            if (produtosDistintos.Count() != ingredientes.Count) {
                context.AddFailure(new FluentValidation.Results.ValidationFailure(
                    "Ingredientes", ResourceMensagensDeErro.RECEITA_INGREDIENTES_REPETIDOS)
                );
            }
        });
    }

}
