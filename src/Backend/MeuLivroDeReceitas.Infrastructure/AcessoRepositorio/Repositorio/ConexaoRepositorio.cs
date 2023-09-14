using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;

public class ConexaoRepositorio : IConexaoReadOnlyRepositorio {
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly MeuLivroDeReceitasContext _context;

    public ConexaoRepositorio(
        MeuLivroDeReceitasContext context,
        IUnidadeDeTrabalho unidadeDeTrabalho
    )
    {
        _context = context;
        _unidadeDeTrabalho = unidadeDeTrabalho;
    }

    public async Task<bool> ExisteConexao(long idUsuarioA, long idUsuarioB) {
        return await _context.Conexoes
            .AnyAsync(
                conexao => conexao.UsuarioId == idUsuarioA && 
                conexao.ConectadoComUsuarioId == idUsuarioB
            );
    }
}
