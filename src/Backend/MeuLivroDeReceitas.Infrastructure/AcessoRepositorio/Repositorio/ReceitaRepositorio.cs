using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;


public class ReceitaRepositorio : 
    IReceitaWriteOnlyRepositorio, 
    IReceitaReadOnlyRepositorio,
    IReceitaUpdateOnlyRepositorio
{

    private readonly MeuLivroDeReceitasContext _context;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

    public ReceitaRepositorio(
        MeuLivroDeReceitasContext context, 
        IUnidadeDeTrabalho unidadeDeTrabalho
    ){
        _context = context;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    async Task<Receita> IReceitaReadOnlyRepositorio.RecuperarPorId(long receitaId) {
        return await _context.Receitas
            .AsNoTracking()
            .Include(receita => receita.Ingredientes)
            .FirstOrDefaultAsync(receita => receita.Id == receitaId);
    }


    async Task<Receita> IReceitaUpdateOnlyRepositorio.RecuperarPorId(long receitaId) {
        return await _context.Receitas
            .Include(receita => receita.Ingredientes)
            .FirstOrDefaultAsync(receita => receita.Id == receitaId);
    }


    public async Task<IList<Receita>> RecuperarTodasDoUsuario(long usuarioId) {

        return await _context.Receitas
            .AsNoTracking()
            .Include(receita => receita.Ingredientes)
            .Where(receita => receita.UsuarioId == usuarioId)
            .ToListAsync();

    }

    public async Task Registrar(Receita receita) {
        await _context.Receitas.AddAsync(receita);
        
    }

    public void Update(Receita receita) {
        _context.Receitas.Update(receita);
    }
}
