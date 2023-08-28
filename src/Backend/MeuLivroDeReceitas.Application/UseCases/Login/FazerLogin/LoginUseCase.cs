using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios;
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
