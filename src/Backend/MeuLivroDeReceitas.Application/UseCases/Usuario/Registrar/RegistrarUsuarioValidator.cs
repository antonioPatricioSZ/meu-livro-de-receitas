using System.Text.RegularExpressions;
using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;



public class RegistrarUsuarioValidator : AbstractValidator<RequisicaoRegistrarUsuarioJson> {

    public RegistrarUsuarioValidator() {

        RuleFor(requisicao => requisicao.Nome).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.NOME_USUARIO_EM_BRANCO);

        RuleFor(requisicao => requisicao.Email).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.EMAIL_USUARIO_EM_BRANCO);

        RuleFor(requisicao => requisicao.Telefone).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.TELEFONE_USUARIO_EM_BRANCO);

        RuleFor(requisicao => requisicao.Senha).NotEmpty()
            .WithMessage(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO);

        // qdo o email estiver em branco, ou tiver cheio de espaços vazios ou for null
        // ele retorna true, aqui estou vendo se é false
        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Email), () => {
            RuleFor(requisicao => requisicao.Email).EmailAddress()
            .WithMessage(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO);
        });

        When(requisicao => !string.IsNullOrWhiteSpace(requisicao.Senha), () => {
            RuleFor(requisicao => requisicao.Senha.Length).GreaterThanOrEqualTo(6)
                .WithMessage(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES);
        });


        When(requisicao => !string.IsNullOrWhiteSpace(requisicao?.Telefone), () => {
            RuleFor(requisicao => requisicao.Telefone).Custom((telefone, context) => {
                var telefonePadrao = "(\\(?\\d{2}\\)?\\s)?(\\d{4,5}\\-\\d{4})";
                var isMatch = Regex.IsMatch(telefone, telefonePadrao);
                if (!isMatch) {
                    context.AddFailure(
                        new FluentValidation.Results.ValidationFailure(
                            nameof(telefone),
                            ResourceMensagensDeErro.TELEFONE_USUARIO_INVALIDO
                        )
                    );
                }
            });
        } );

    }

}
