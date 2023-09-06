using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;


public class AtualizarReceitaUseCase : IAtualizarReceitaUseCase {

    private readonly IReceitaUpdateOnlyRepositorio _repositorio;
    private readonly IMapper _mapper;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IUsuarioLogado _usuarioLogado;

    public AtualizarReceitaUseCase(
        IReceitaUpdateOnlyRepositorio repositorio,
        IMapper mapper, 
        IUnidadeDeTrabalho unidadeDeTrabalho,
        IUsuarioLogado usuarioLogado
    ){
        _repositorio = repositorio;
        _mapper = mapper;
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _usuarioLogado = usuarioLogado;
    }

    public async Task Executar(long id, RequisicaoReceitaJson requisicao) {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var receita = await _repositorio.RecuperarPorId(id);

        Validar(usuarioLogado, receita, requisicao);

        _mapper.Map(requisicao, receita);

        _repositorio.Update(receita);

        await _unidadeDeTrabalho.Commit();
    }


    public static void Validar(
        Domain.Entidades.Usuario usuarioLogado,
        Domain.Entidades.Receita receita,
        RequisicaoReceitaJson requisicao
    ){
        if(receita is null || receita.UsuarioId != usuarioLogado.Id) {
            var mensagensDeErro = new List<string> {
                ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA
            };
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }

        var validator = new AtualizarReceitaValidator();
        var resultado = validator.Validate(requisicao);

        if (!resultado.IsValid) {
            var mensagensDeErro = resultado.Errors.Select(erro => erro.ErrorMessage).ToList();
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }

    }

}
