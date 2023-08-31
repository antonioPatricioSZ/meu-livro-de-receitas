using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;


public class AlterarSenhaUseCase : IAlterarSenhaUseCase {

    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUsuarioUpdateOnlyRepositorio _repositorio;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;


    public AlterarSenhaUseCase(
        IUsuarioLogado usuarioLogado,
        IUsuarioUpdateOnlyRepositorio repositorio,
        EncriptadorDeSenha encriptadorDeSenha,
        IUnidadeDeTrabalho unidadeDeTrabalho
    )
    {
        _usuarioLogado = usuarioLogado;
        _repositorio = repositorio;
        _encriptadorDeSenha = encriptadorDeSenha;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Executar(RequisicaoAlterarSenhaJson requisicao) {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        // Aqui ele não pode ser atualizado pq estou usando o AsNoTracking()

        var usuario = await _repositorio.RecuperarPorId(usuarioLogado.Id);
        // Por isso faço isso aqui para pode atualizar a senha do usuário

        Validar(requisicao, usuario);

        usuario.Senha = _encriptadorDeSenha.Criptografar(requisicao.NovaSenha);

        _repositorio.Update(usuario);
        await _unidadeDeTrabalho.Commit();

    }


    private void Validar(RequisicaoAlterarSenhaJson requisicao, Domain.Entidades.Usuario usuario) {
        var validator = new AlterarSenhaValidator();

        var resultado = validator.Validate(requisicao);

        var senhaAtualCriptografada = _encriptadorDeSenha.Criptografar(requisicao.SenhaAtual);

        if(!usuario.Senha.Equals(senhaAtualCriptografada)) {
            resultado.Errors.Add(
                new FluentValidation.Results.ValidationFailure(
                    "senhaAtual",
                    ResourceMensagensDeErro.SENHA_ATUAL_INVALIDA
                )
            );
        }

        if(!resultado.IsValid) {
            var mensagens = resultado.Errors.Select(erro => erro.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagens);
        }
    }

}
