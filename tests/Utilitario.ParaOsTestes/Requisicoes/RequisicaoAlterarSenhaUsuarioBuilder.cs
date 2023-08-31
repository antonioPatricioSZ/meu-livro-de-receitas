using Bogus;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaOsTestes.Requisicoes;

public class RequisicaoAlterarSenhaUsuarioBuilder {

    public static RequisicaoAlterarSenhaJson Construir(int tamanhoSenha = 10) {
        return new Faker<RequisicaoAlterarSenhaJson>()
            .RuleFor(requisicao => requisicao.SenhaAtual, f => f.Internet.Password(10))
            .RuleFor(requisicao => requisicao.NovaSenha, f => f.Internet.Password(tamanhoSenha));
    }

}
