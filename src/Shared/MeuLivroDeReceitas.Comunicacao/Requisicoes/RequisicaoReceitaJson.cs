using MeuLivroDeReceitas.Comunicacao.Enum;

namespace MeuLivroDeReceitas.Comunicacao.Requisicoes;


public class RequisicaoReceitaJson {

    // classes que tem propriedades de lista no case de requisicoes e respostas inicar elas

    public RequisicaoReceitaJson() {
        Ingredientes = new();
        // na ver 6 no dotnet nao precisa fazer assim
        // Ingredientes = List<RequisicaoRegistrarIngredienteJson>();
    }

    public string Titulo { get; set; }
    public Categoria Categoria { get; set; }
    public string ModoPreparo { get; set; }
    public int TempoPreparo { get; set; }
    public List<RequisicaoIngredienteJson> Ingredientes { get; set; }

}
