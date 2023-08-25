using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace Validators.Test.Usuario.Registrar;


public class RegistrarUsuarioValidatorTest {

    // A primeira funcao é bom colocar como sucesso
    [Fact]
    public void Validar_Sucesso()
    {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeTrue();

    }


    [Fact]
    public void Validar_Erro_Nome_Vazio() {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();
        requisicao.Nome = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.NOME_USUARIO_EM_BRANCO)
        );

    }



    [Fact]
    public void Validar_Erro_Email_Vazio()
    {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();
        requisicao.Email = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_EM_BRANCO)
        );

    }



    [Fact]
    public void Validar_Erro_Senha_Vazia()
    {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();
        requisicao.Senha = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO)
        );

    }


    [Fact]
    public void Validar_Erro_Telefone_Vazio()
    {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();
        requisicao.Telefone = string.Empty;

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_EM_BRANCO)
        );

    }




    [Fact]
    public void Validar_Erro_Email_Invalido()
    {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();
        requisicao.Email = "patricio";

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.EMAIL_USUARIO_INVALIDO)
        );

    }



    [Fact]
    public void Validar_Erro_Telefone_Invalido()
    {
        // colocar o nome da funcao com o erro esperado

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir();
        requisicao.Telefone = "239";

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.TELEFONE_USUARIO_INVALIDO)
        );

    }



    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validar_Erro_Senha_Invalida(int tamanhoSenha)   {
        // O tamnho da senha vai ser o valor que no InlineData que é executado cinco vezes nesse teste
        // colocar o nome da funcao com o erro esperado
        // Os testes são executados de forma aleatoria

        var validator = new RegistrarUsuarioValidator();

        var requisicao = RequisicaoResgistrarUsuarioBuilder.Construir(tamanhoSenha);

        var resultado = validator.Validate(requisicao);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().ContainSingle().And.Contain(
            erro => erro.ErrorMessage.Equals(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES)
        );

    }

}
