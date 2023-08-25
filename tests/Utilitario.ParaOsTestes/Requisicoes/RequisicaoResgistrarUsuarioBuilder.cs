using Bogus;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaOsTestes.Requisicoes;


public class RequisicaoResgistrarUsuarioBuilder {

    public static RequisicaoRegistrarUsuarioJson Construir(int tamanho = 10) {

        return new Faker<RequisicaoRegistrarUsuarioJson>()
            .RuleFor(requisicao => requisicao.Nome, f => f.Person.FullName)
            .RuleFor(requisicao => requisicao.Email, f => f.Internet.Email())
            .RuleFor(requisicao => requisicao.Senha, f => f.Internet.Password(tamanho))
            .RuleFor(
                requisicao => requisicao.Telefone, f => f.Phone.PhoneNumber(
                    "!#9########"
                ).Replace("!", $"{f.Random.Int(min: 1, max: 9)}")
            );
    }

}
