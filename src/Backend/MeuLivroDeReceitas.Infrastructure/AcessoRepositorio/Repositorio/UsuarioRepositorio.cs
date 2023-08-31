using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Usuario;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infrastructure.AcessoRepositorio.Repositorio;


public class UsuarioRepositorio : 
    IUsuarioWriteOnlyRepositorio, 
    IUsuarioReadOnlyRepositorio, 
    IUsuarioUpdateOnlyRepositorio {

    private readonly MeuLivroDeReceitasContext _context;

    public UsuarioRepositorio(MeuLivroDeReceitasContext context) {
        _context = context;
    }

    public async Task Adicionar(Usuario usuario) {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<bool> ExisteUsuarioComEmail(string email) {
        var result = await _context.Usuarios.AnyAsync(usuario => usuario.Email.Equals(email));
        return result;
    }

    public async Task<Usuario> Login(string email, string senha) {
        return await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(
            usuario => usuario.Email.Equals(email) && usuario.Senha.Equals(senha)
        );
    }

    public async Task<Usuario> RecuperarPorEmail(string email) {
        return await _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Email.Equals(email));
        // qdo for retornar uma entidade que a funcao é apenas leitura usar o AsNoTracking()
    }

    public async Task<Usuario> RecuperarPorId(long id) {
        return await _context.Usuarios
           .FirstOrDefaultAsync(usuario => usuario.Id == id);
        // Usar o Equals() qdo for uma string
    }

    public void Update(Usuario usuario) {
        _context.Usuarios.Update(usuario);
    }
}

// O usuário salvou com sucesso. E o contato de emergência quando foi colocar na tabela.
// O desenvolvedor se equivocou e fez uma validação errada. Diferente do banco de dados
// com número de caracteres do nome, por exemplo. E deu erro. E o que isso vai acontecer?
// Vai fazer com que fique um usuário no banco de dados na tabela usuário, mas sem um contato
// de emergência, isso pode dar erro. A exceção vai ser lançada, obviamente, mas ele vai 
// deixar lá no banco de dados. Nós vamos encontrar uma forma de isso não acontecer.
// A forma que nós vamos fazer é utilizar o commit. Ou seja, se tudo der certo, você
// persiste a informação no banco de dados, senão não vai fazer nada. Ou seja, a gente 
// vai abrir a conexão com o Banco de dados e fazer o commit. Se tudo der certo, persiste
// todas as informações. Se algo dá errado, nenhuma das informações e nenhuma tabela 
// que foi utilizada nessa conexão vai persistir. O banco de dados vai apagar tudo,
// não vai nem criar. Como a gente vai fazer esse controle? Nós chamamos.
// É um senso bem comum de chamamors de unit of work, ou seja, uma unidade de trabalho.