using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;


public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio {

    private readonly MeuLivroDeReceitasContext _context;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public ReceitaRepositorio(
        MeuLivroDeReceitasContext context, 
        IUnidadeDeTrabalho unidadeDeTrabalho
    ){
        _context = context;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task Registrar(Receita receita) {
        await _context.Receitas.AddAsync(receita);
        
    }
}
