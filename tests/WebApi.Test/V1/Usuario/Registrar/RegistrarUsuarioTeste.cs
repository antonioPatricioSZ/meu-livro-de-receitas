using System.Net;
using System.Text.Json;
using FluentAssertions;
using MeuLivroDeReceitas.Exceptions;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Usuario.Registrar;


public class RegistrarUsuarioTeste : ControllerBase {

    private const string METODO = "usuario";

    public RegistrarUsuarioTeste(MeuLivroReceitasWebApplicationFactory<Program> factory) : base(factory)
    {}


    [Fact]
    public async Task Validar_Sucesso() {
        var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
        var respota = await PostRequest(METODO, requisicao);

        respota.StatusCode.Should().Be(HttpStatusCode.Created);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        respostaData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task Validar_Erro_Nome_Vazio()
    {
        var requisicao = RequisicaoRegistrarUsuarioBuilder.Construir();
        requisicao.Nome = string.Empty;
        var respota = await PostRequest(METODO, requisicao);

        respota.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(
            erro => erro.GetString().Equals(ResourceMensagensDeErro.NOME_USUARIO_EM_BRANCO)
        );
    }

}
