﻿using MeuLivroDeReceitas.Api.Binder;
using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;
[Route("[controller]")]
[ApiController]
[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ReceitasController : ControllerBase {
   
    [HttpPost]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Registrar(
        [FromServices] IRegistrarReceitaUseCase useCase,
        RequisicaoReceitaJson requisicao
    ){

        var resposta = await useCase.Executar(requisicao);

        return Created(string.Empty, resposta);

    }


    [HttpGet]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorId(
        [FromServices] IRecuperarReceitaPorIdUseCase useCase,
        [FromRoute] [ModelBinder(typeof(HashidsModelBinder))] long id
    ){

        var resposta = await useCase.Executar(id);

        return Ok(resposta);

    }

}