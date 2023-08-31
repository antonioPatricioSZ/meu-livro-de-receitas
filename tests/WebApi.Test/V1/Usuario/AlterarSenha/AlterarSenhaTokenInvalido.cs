using System.Net;
using FluentAssertions;
using Utilitario.ParaOsTestes.Requisicoes;
using Utilitario.ParaOsTestes.Token;
using Xunit;

namespace WebApi.Test.V1.Usuario.AlterarSenha;


public class AlterarSenhaTokenInvalido : ControllerBase
{

    private const string METODO = "usuario/alterar-senha";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public AlterarSenhaTokenInvalido(
        MeuLivroReceitasWebApplicationFactory<Program> factory
    ): base(factory) {
        _usuario = factory.RecuperUsuario();
        _senha = factory.RecuperSenha();
    }


    [Fact]
    public async Task Validar_Erro_Token_Vazio() {

        var token = string.Empty;

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;

        var respota = await PutRequest(METODO, requisicao, token);

        respota.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    }



    [Fact]
    public async Task Validar_Erro_Token_Expirado() {

        var token = TokenControllerBuilder.TokenExpirado().GerarToken(_usuario.Email);
        Thread.Sleep(1000);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;

        var respota = await PutRequest(METODO, requisicao, token);

        respota.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    }


    [Fact]
    public async Task Validar_Erro_Token_Usuario_Fake() {

        var token = TokenControllerBuilder.Instancia().GerarToken("usuario@fake.com");

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;

        var respota = await PutRequest(METODO, requisicao, token);

        respota.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    }


}
