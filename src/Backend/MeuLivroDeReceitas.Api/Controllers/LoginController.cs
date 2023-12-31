﻿using MeuLivroDeReceitas.Application.UseCases.Login.FazerLogin;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;


[Route("[controller]")]
[ApiController]
public class LoginController : ControllerBase {

    
    [HttpPost]
    [ProducesResponseType(typeof(RespostaLoginJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase useCase,
        RequisicaoLoginJson requisicao
    ){

        var resposta = await useCase.Executar(requisicao);

        return Ok(resposta);

    }

}
