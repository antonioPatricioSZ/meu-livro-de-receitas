using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using MeuLivroDeReceitas.Exceptions;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Deletar;


public class DeletarReceitaUseCase : IDeletarReceitaUseCase {

    private readonly IReceitaReadOnlyRepositorio _repositorioReadOnly;
    private readonly IReceitaWriteOnlyRepositorio _repositorioWriteOnly;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IUsuarioLogado _usuarioLogado;

    public DeletarReceitaUseCase(
        IUnidadeDeTrabalho unidadeDeTrabalho,
        IUsuarioLogado usuarioLogado,
        IReceitaReadOnlyRepositorio repositorioReadOnly,
        IReceitaWriteOnlyRepositorio repositorioWriteOnly
    )
    {
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _usuarioLogado = usuarioLogado;
        _repositorioReadOnly = repositorioReadOnly;
        _repositorioWriteOnly = repositorioWriteOnly;
    }

    public async Task Executar(long receitaId) {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var receita = await _repositorioReadOnly.RecuperarPorId(receitaId);

        Validar(usuarioLogado, receita);

        await _repositorioWriteOnly.Deletar(receitaId);

        await _unidadeDeTrabalho.Commit();
    }


    public static void Validar(
        Domain.Entidades.Usuario usuarioLogado,
        Domain.Entidades.Receita receita
    ){

        if (receita is null || receita.UsuarioId != usuarioLogado.Id) {
            
            var mensagensDeErro = new List<string> {
                ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA
            };
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }

    }

}
