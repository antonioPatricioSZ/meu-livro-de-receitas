﻿using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;


public class RecuperarReceitaPorIdUseCase : IRecuperarReceitaPorIdUseCase
{

    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public RecuperarReceitaPorIdUseCase(
        IReceitaReadOnlyRepositorio repositorio,
        IUsuarioLogado usuarioLogado,
        IMapper mapper
    )
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
    }


    public async Task<RespostaReceitaJson> Executar(long id)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receita = await _repositorio.RecuperarPorId(id);

        Validar(usuarioLogado, receita);

        return _mapper.Map<RespostaReceitaJson>(receita);

    }


    public static void Validar(
        Domain.Entidades.Usuario usuarioLogado,
        Domain.Entidades.Receita receita
    ){
        
        if(receita is null || receita.UsuarioId != usuarioLogado.Id) {
            // colocar as validacoes nessa ordem, pois de mudar para verificar 
            // se o usuario é o mesmo da receita e dps se a receita é null ele
            // retornara uma exception, então a ordem de validacao importa
            var mensagensDeErro = new List<string> { 
                ResourceMensagensDeErro.RECEITA_NAO_ENCONTRADA 
            };
            throw new ErrosDeValidacaoException(mensagensDeErro);
        }
    
    } 

}
