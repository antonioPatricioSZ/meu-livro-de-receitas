using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Usuario.AlterarSenha;


public interface IAlterarSenhaUseCase {

    Task Executar(RequisicaoAlterarSenhaJson requisicao);
    // Na alteração de senha não precisa retornar nada

}
