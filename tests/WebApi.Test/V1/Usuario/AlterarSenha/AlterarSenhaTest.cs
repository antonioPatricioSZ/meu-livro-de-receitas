using FluentAssertions;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;
using System.Net;
using System.Text.Json;
using Utilitario.ParaOsTestes.Requisicoes;
using Xunit;

namespace WebApi.Test.V1.Usuario.AlterarSenha;

public class AlterarSenhaTest : ControllerBase {

    private const string METODO = "usuario/alterar-senha";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;

    public AlterarSenhaTest(MeuLivroReceitasWebApplicationFactory<Program> factory) : base(factory) {
        _usuario = factory.RecuperUsuario();
        _senha = factory.RecuperSenha();
    }


    [Fact]
    public async Task Validar_Sucesso()  {

        var token = await Login(_usuario.Email, _senha);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;

        var respota = await PutRequest(METODO, requisicao, token);

        respota.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
    }


    [Fact]
    public async Task Validar_Erro_NovaSenha_EmBranco() {

        var token = await Login(_usuario.Email, _senha);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = _senha;
        requisicao.NovaSenha = string.Empty;

        var respota = await PutRequest(METODO, requisicao, token);

        respota.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();
        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        var erros = respostaData.RootElement.GetProperty("mensagens").EnumerateArray();
        erros.Should().ContainSingle().And.Contain(
            erro => erro.GetString().Equals(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO)
        );

    }

}
