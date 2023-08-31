using System.Net;
using System.Text.Json;
using FluentAssertions;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;
using Xunit;

namespace WebApi.Test.V1.Login.FazerLogin;



public class LoginTest : ControllerBase {  

    private const string METODO = "login";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public LoginTest(MeuLivroReceitasWebApplicationFactory<Program> factory) : base(factory) {
        _usuario = factory.RecuperUsuario();
        _senha = factory.RecuperSenha();
    }


    [Fact]
    public async Task Validar_Sucesso() {

        var requisicao = new RequisicaoLoginJson { 
            Email = _usuario.Email,
            Senha = _senha
        };
        var respota = await PostRequest(METODO, requisicao);

        respota.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        respostaData.RootElement.GetProperty("nome").GetString().Should()
            .NotBeNullOrWhiteSpace().And.Be(_usuario.Nome);

        respostaData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Validar_Erro_Senha_Invalida() {

        var requisicao = new RequisicaoLoginJson {
            Email = _usuario.Email,
            Senha = "senhaInvalida"
        };
        var respota = await PostRequest(METODO, requisicao);

        respota.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(
            erro => erro.GetString().Equals(ResourceMensagensDeErro.LOGIN_INVALIDO)
        );

    }


    [Fact]
    public async Task Validar_Erro_Email_Invalido() {

        var requisicao = new RequisicaoLoginJson {
            Email = "email@invalido.com",
            Senha = _senha
        };
        var respota = await PostRequest(METODO, requisicao);

        respota.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(
            erro => erro.GetString().Equals(ResourceMensagensDeErro.LOGIN_INVALIDO)
        );

    }


    [Fact]
    public async Task Validar_Erro_Email_Senha_Invalido() {
        var requisicao = new RequisicaoLoginJson
        {
            Email = "email@invalido.com",
            Senha = "senhaInvalida"
        };
        var respota = await PostRequest(METODO, requisicao);

        respota.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(
            erro => erro.GetString().Equals(ResourceMensagensDeErro.LOGIN_INVALIDO)
        );

    }

}
