using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;

public class AtualizarReceitaValidator : AbstractValidator<RequisicaoReceitaJson> {

    public AtualizarReceitaValidator() {
        RuleFor(requisicao => requisicao).SetValidator(new ReceitaValidator());
    }

}
