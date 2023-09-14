using MeuLivroDeReceitas.Application.Servicos.Token;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using Microsoft.AspNetCore.Authorization;

namespace MeuLivroDeReceitas.Api.Filtros.UsuarioLogado;


public class UsuarioLogadoHandler : AuthorizationHandler<UsuarioLogadoRequirement> {

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenController _tokenController;
    private readonly IUsuarioReadOnlyRepositorio _repositorio;

    public UsuarioLogadoHandler(
        IHttpContextAccessor httpContextAccessor,
        TokenController tokenController,
        IUsuarioReadOnlyRepositorio repositorio
    ){
        _httpContextAccessor = httpContextAccessor;
        _tokenController = tokenController;
        _repositorio = repositorio;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        UsuarioLogadoRequirement requirement
    ){
        try {
            var authorization = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
             .ToString();

            if (string.IsNullOrWhiteSpace(authorization)) {
                context.Fail();
                return;
            }
            // verifica se o token veio e se não está vazio, se tiver assim recusa a conexao

            var token = authorization["Bearer".Length..].Trim();

            var emailUsuario = _tokenController.RecuperarEmail(token);
            // verifica se o token é valido se n for vai pro catch e recusa a conexao

            var usuario = await _repositorio.RecuperarPorEmail(emailUsuario);

            if (usuario is null) {
                context.Fail();
                return;
                // se o usuario for null recusa a conexao
            }

            context.Succeed(requirement);
            // se tudo tiver certo aceita a conexao

        } catch {
            context.Fail();
        }
    }

}
