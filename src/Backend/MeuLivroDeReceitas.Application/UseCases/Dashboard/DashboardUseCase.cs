using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Dashboard;

public class DashboardUseCase : IDashboardUseCase {

    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public DashboardUseCase(
        IReceitaReadOnlyRepositorio repositorio,
        IUsuarioLogado usuarioLogado,
        IMapper mapper
    ){
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
    }


    public async Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson requisicao) {

        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var receitas = await _repositorio.RecuperarTodasDoUsuario(usuarioLogado.Id);

        receitas = Filtrar(requisicao, receitas);

        return new RespostaDashboardJson {
            Receitas = _mapper.Map<List<RespostaReceitaDashboardJson>>(receitas)
        };

    }

    private static IList<Domain.Entidades.Receita> Filtrar(
        RequisicaoDashboardJson requisicao,
    IList<Domain.Entidades.Receita> receitas
    ){

        var receitaFiltradas = receitas;

        if(requisicao.Categoria.HasValue) {
            receitaFiltradas = receitas.Where(
                receita => receita.Categoria == (Domain.Enum.Categoria)requisicao.Categoria.Value
            ).ToList();
        }

        if(!string.IsNullOrWhiteSpace(requisicao.TituloOuIngrediente)) {
            receitaFiltradas = receitas.Where(
                receita => receita.Titulo.Contains(requisicao.TituloOuIngrediente) ||
                receita.Ingredientes.Any(
                    ingrediente => ingrediente.Produto.Contains(requisicao.TituloOuIngrediente)
                )
            ).ToList();
        }

        return receitaFiltradas;

        // Então, como eu disse, se a gente está usando o exemplo de um chocolate, 
        // por exemplo, ou o título vai conter a palavra chocolate, então bolo de chocolate,
        // se estiver no título, vai ter que churros de chocolate vai estar aqui ou algum
        // ingrediente possui um produto com essa string foi passada agora.
        // Assim a gente pode devolver essa receitas filtradas. Por que eu gosto de fazer
        // essa linha que é importante fazer? Eu gosto de fazer isso porque não necessariamente.
        // O primeiro é que vai entrar, pode entrar apenas no segundo e fazendo essa
        // igualdade aqui, eu sempre garanto que essa variável começa com o mesmo, 
        // os mesmos valores da nossa receita e da nossa lista de receitas originais.
        // E aí, já fazendo filtro após filtros nela, então pode ter todos os filtros.
        // Os dois pode ter só o primeiro e não o segundo, ou só o segundo e não primeiro, 
        // ou pode não conter nenhum. Então eu gosto de fazer isso porque a gente evita alguns
        // pequenos errinhos.Aí que pode acontecer a que eles já vão preparar para funcionar.

    }

}
