using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class CodigoRepositorio : ICodigoWriteOnlyRepositorio, ICodigoReadOnlyRepositorio {

    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly MeuLivroDeReceitasContext _context;

    public CodigoRepositorio(
        MeuLivroDeReceitasContext context,
        IUnidadeDeTrabalho unidadeDeTrabalho
    ){
        _context = context;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task<Codigos> RecuperarEntidadeCodigo(string codigo) {
        return await _context.Codigos.AsNoTracking().FirstOrDefaultAsync(c => c.Codigo == codigo);
    }

    public async Task Registrar(Codigos codigo) {

        var codigoBancoDeDados = await _context.Codigos
            .FirstOrDefaultAsync(cod => cod.UsuarioId == codigo.UsuarioId);
        // verifico se já existe um codigo para este usuario no banco de dados

        if(codigoBancoDeDados is not  null) {
            // se existir eu vou apenas atualizar esse codigo
            codigoBancoDeDados.Codigo = codigo.Codigo;
            _context.Codigos.Update(codigoBancoDeDados);
        } else {
            // se não existir eu crio um
            await _context.Codigos.AddAsync(codigo);
        }

    }
}
