using MeuLivroDeReceitas.Domain.Repositorios;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio;


public sealed class UnidadeDeTrabalho : IDisposable, IUnidadeDeTrabalho {

    private readonly MeuLivroDeReceitasContext _context;
    private bool _disposed;
    // verifica se já foi liberado memoria pra isso

    public UnidadeDeTrabalho(MeuLivroDeReceitasContext context) {
        _context = context;
    }

    public async Task Commit() {
       await _context.SaveChangesAsync();
    }
    // qdo essa funcao for chamada nós estaremos utilizando a funcao da interface
    // que é IUnidadeDeTrabalho, assim nós não dependeremos de instanciar essa classe

    public void Dispose() {
        Dispose(true);
    }

    // Isso é feito para liberar a memoria que não está sendo utilizada referente a essa
    // unidade de trabalho
    private void Dispose(bool disposing) { 
        
        if (!_disposed && disposing) {
            _context.Dispose();
            // estou chamando ela par ser executada (Dispose) faz a liberação de memória
        }
        _disposed = true;
    }
}
