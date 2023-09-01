namespace MeuLivroDeReceitas.Domain.Entidades;


public class Ingrediente : EntidadeBase {

    public string Produto { get; set; }
    public string Quantidade { get; set; }
    public string ReceitaId { get; set; }
    //public Receita Receita { get; set; }

}
