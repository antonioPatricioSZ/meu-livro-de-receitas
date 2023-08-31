using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;



public class LoginUseCase : ILoginUseCase {


    private readonly IUsuarioReadOnlyRepositorio _usuarioReadOnlyRepositorio;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly TokenController _tokenController;

    public LoginUseCase(
        EncriptadorDeSenha encriptadorDeSenha,
        TokenController tokenController,
        IUsuarioReadOnlyRepositorio usuarioReadOnlyRepositorio
    )
    {
        _encriptadorDeSenha = encriptadorDeSenha;
        _tokenController = tokenController;
        _usuarioReadOnlyRepositorio = usuarioReadOnlyRepositorio;
    }

    public async Task<RespostaLoginJson> Executar(RequisicaoLoginJson requisicao) {

        var senhaCriptografada = _encriptadorDeSenha.Criptografar(requisicao.Senha);
        // Eu posso fazer a validação ou não dos useCases
        // Eu criptografo a senha, uso a funcao que acessa o bancdo de dados passando os parametros
        // se o usuario for null (ou seja, nao tiver nenhum usuario com o email e senhas iguas
        // aos dados passados) eu lanco uma exception, e nesse caso eu criei outra exception pois
        // ela retorna 401 de status e nao mais o 400 que era pq os dados estavam invalidos,
        // 401 pq não tem autorização pois ele forneceu as credenciais invalidas
        // mas se estiver tudo ok com os dados passados eu retorno o nome do usuario e o token

        var usuario = await _usuarioReadOnlyRepositorio.Login(requisicao.Email, senhaCriptografada);

        if(usuario == null) {
            throw new LoginInvalidoException();
        }

        return new RespostaLoginJson {
            Nome = usuario.Nome,
            Token = _tokenController.GerarToken(usuario.Email) 
        };

    }
}
