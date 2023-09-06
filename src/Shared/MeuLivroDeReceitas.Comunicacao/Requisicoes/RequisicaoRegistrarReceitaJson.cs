using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Requisicoes;


public class RequisicaoRegistrarReceitaJson {

    // classes que tem propriedades de lista no case de requisicoes e respostas inicar elas

    public RequisicaoRegistrarReceitaJson() {
        Ingredientes = new();
        // na ver 6 no dotnet nao precisa fazer assim
        // Ingredientes = List<RequisicaoRegistrarIngredienteJson>();
    }

    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public List<RequisicaoRegistrarIngredienteJson> Ingredientes { get; set; }

}
