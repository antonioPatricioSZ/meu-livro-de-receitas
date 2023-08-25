using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;


public class RegistrarUsuarioUseCase : IRegistrarUsuarioUseCase {

    private readonly IUsuarioReadOnlyRepositorio _usuarioReadOnlyRepositorio;
    private readonly IUsuarioWriteOnlyRepositorio _repositorio;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly EncriptadorDeSenha _encriptadorDeSenha;
    private readonly TokenController _tokenController;

    public RegistrarUsuarioUseCase(
        IUsuarioWriteOnlyRepositorio repositorio,
        IMapper mapper,
        IUnidadeDeTrabalho unidadeDeTrabalho,
        EncriptadorDeSenha encriptadorDeSenha,
        TokenController tokenController,
        IUsuarioReadOnlyRepositorio usuarioReadOnlyRepositorio
    )
    {
        _repositorio = repositorio;
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _encriptadorDeSenha = encriptadorDeSenha;
        _tokenController = tokenController;
        _usuarioReadOnlyRepositorio = usuarioReadOnlyRepositorio;
    }

    public async Task<RespostaUsuarioRegistradoJson> Executar(RequisicaoRegistrarUsuarioJson requisicao) {

        await Validar(requisicao);

        var entidade = _mapper.Map<Domain.Entidades.Usuario>(requisicao);
        entidade.Senha = _encriptadorDeSenha.Criptografar(requisicao.Senha);
        // aqui vamos fazer a conversão (requisicao) de requisicao para Usuario

        await _repositorio.Adicionar(entidade);
        await _unidadeDeTrabalho.Commit();

        var token = _tokenController.GerarToken(entidade.Email);

        return new RespostaUsuarioRegistradoJson { Token = token };


    }

    private async Task Validar(RequisicaoRegistrarUsuarioJson requisicao) { 
        
        var validator = new RegistrarUsuarioValidator();
        var resultado = validator.Validate(requisicao);

        // se for válido é pq não tem erro

        var existeUsuarioComEmail = await _usuarioReadOnlyRepositorio.ExisteUsuarioComEmail(requisicao.Email);

        if(existeUsuarioComEmail) {
            resultado.Errors.Add(
                new FluentValidation.Results.ValidationFailure(
                    "email", ResourceMensagensDeErro.EMAIL_JA_REGISTRADO
                )
            );
        }

        if(!resultado.IsValid) {
            var mensagensDeErro = resultado.Errors.Select(erro => erro.ErrorMessage).ToList();
            // aqui eu pego apenas as mensagens de erro a partir do array de objetos,
            // mas para cada objeto eu pego só a mensagem de erro
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }

    }

}
