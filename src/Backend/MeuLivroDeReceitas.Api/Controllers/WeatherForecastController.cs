using MeuLivroDeReceitas.Application.UseCases.Usuario.Registrar;
using MeuLivroDeReceitas.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase {

    [HttpGet]
    public async Task<IActionResult> Get([FromServices] IRegistrarUsuarioUseCase useCase) {

        var resposta = await useCase.Executar(new Comunicacao.Requisicoes.RequisicaoRegistrarUsuarioJson
        {
            Email = "patricio@gmail.com",
            Nome = "Patricio",
            Senha = "123456",
            Telefone = "34 9 9876-9843"
        });
        return Ok(resposta);
    }
}
