using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace Validators.Test.Usuario.AlterarSenha;


public class AlterarSenhaValidatorTest {


    [Fact]
    public void Validar_Sucesso() {
        // colocar o nome da funcao com o erro esperado

        var validator = new AlterarSenhaValidator();

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeTrue();

    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validar_Erro_Senha_Invalida(int tamanhoSenha) {
        // O tamnho da senha vai ser o valor que no InlineData que é executado cinco vezes nesse teste
        // colocar o nome da funcao com o erro esperado
        // Os testes são executados de forma aleatoria

        var validator = new AlterarSenhaValidator();

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES)
        );

    }


    [Fact]
    public void Validar_Erro_Senha_Vazia() {
        // colocar o nome da funcao com o erro esperado

        var validator = new AlterarSenhaValidator();

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.NovaSenha = string.Empty;
        requisicao.SenhaAtual = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO)
        );

    }

}
