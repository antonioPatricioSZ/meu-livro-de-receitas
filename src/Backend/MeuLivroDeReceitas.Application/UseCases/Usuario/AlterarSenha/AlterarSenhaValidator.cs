using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;

public class AlterarSenhaValidator : AbstractValidator<RequisicaoAlterarSenhaJson> {

    public AlterarSenhaValidator() {
        RuleFor(requisicao => requisicao.NovaSenha).SetValidator(new SenhaValidator());
    }

}
