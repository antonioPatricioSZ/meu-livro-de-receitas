using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase {

    [HttpPost]
    [ProducesResponseType(typeof(RespostaUsuarioRegistradoJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> AdicionarUsuario(
        [FromServices] IRegistrarUsuarioUseCase useCase,
        RequisicaoRegistrarUsuarioJson requisicao
    ){

        var resultado = await useCase.Executar(requisicao);

        return Created(string.Empty, resultado);
       
    }
}
