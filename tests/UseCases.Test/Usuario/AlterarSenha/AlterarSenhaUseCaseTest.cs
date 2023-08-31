using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Utilitario.ParaOsTestes.Criptografia;
using Utilitario.ParaOsTestes.Entidades;
using Utilitario.ParaOsTestes.Repositorios;
using Utilitario.ParaOsTestes.Requisicoes;
using Utilitario.ParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Usuario.AlterarSenha;


public class AlterarSenhaUseCaseTest {


    [Fact]
    public async Task Validar_Sucesso() {

        (var usuario, string senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () => {
            await useCase.Executar(requisicao);
        };

        await acao.Should().NotThrowAsync();
        // não deve retornar uma Exception
    }

    [Fact]
    public async Task Validar_Erro_NovaSenha_EmBranco() {

        (var usuario, string senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        Func<Task> acao = async () => {
            await useCase.Executar(new RequisicaoAlterarSenhaJson
            {
                SenhaAtual = senha,
                NovaSenha = ""
            });
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(
                exception => exception.MensagensDeErro.Count == 1 && 
                exception.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_EM_BRANCO)
            );
        // Deve retornar uma exception do tipo ErrosDeValidacaoException, ondex deve ter apenas
        // uma mensagem de erro e a mensagem de erro deve ser SENHA_USUARIO_EM_BRANCO
    }



    [Fact]
    public async Task Validar_Erro_SenhaAtual_Invalida()
    {

        (var usuario, string senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);
        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir();
        requisicao.SenhaAtual = "senhaInvalida";


        Func<Task> acao = async () => {
            await useCase.Executar(requisicao);
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(
                exception => exception.MensagensDeErro.Count == 1 &&
                exception.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA)
            );
        // Deve retornar uma exception do tipo ErrosDeValidacaoException, ondex deve ter apenas
        // uma mensagem de erro e a mensagem de erro deve ser SENHA_USUARIO_EM_BRANCO
    }



    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task Validar_Erro_SenhaAtual_Minimo_Caracteres(int tamanhoSenha = 10) {

        (var usuario, string senha) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var requisicao = RequisicaoAlterarSenhaUsuarioBuilder.Construir(tamanhoSenha);
        requisicao.SenhaAtual = senha;

        Func<Task> acao = async () => {
            await useCase.Executar(requisicao);
        };

        await acao.Should().ThrowAsync<ErrosDeValidacaoException>()
            .Where(
                exception => exception.MensagensDeErro.Count == 1 &&
                exception.MensagensDeErro.Contains(ResourceMensagensDeErro.SENHA_USUARIO_MINIMO_SEIS_CARACTERES)
            );
        // Deve retornar uma exception do tipo ErrosDeValidacaoException, ondex deve ter apenas
        // uma mensagem de erro e a mensagem de erro deve ser SENHA_ATUAL_INVALIDA
    }


    private static AlterarSenhaUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario) {

        var encriptadorDeSenha = EncriptadorDeSenhaBuilder.Instancia();
        var unidadeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var repositorio = UsuarioUpdateOnlyRepositorioBuilder.Instancia().RecuperarPorId(usuario).Construir();
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();


        return new AlterarSenhaUseCase(usuarioLogado, repositorio, encriptadorDeSenha, unidadeTrabalho);
    }

}
