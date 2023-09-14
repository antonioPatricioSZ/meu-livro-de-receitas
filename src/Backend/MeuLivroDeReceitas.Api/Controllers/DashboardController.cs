using MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;
using MeuLivroDeReceitas.Application.UseCases.Dashboard;
using MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class DashboardController : ControllerBase {
 

    [HttpPut]
    [ProducesResponseType(typeof(RespostaDashboardJson) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
    public async Task<IActionResult> RecuperarDashboard(
        [FromServices] IDashboardUseCase useCase,
        RequisicaoDashboardJson requisicao
    ){

        var resultado = await useCase.Executar(requisicao);

        if(resultado.Receitas.Any()) {
            return Ok(resultado);
        }

        return NoContent();

    }

}
