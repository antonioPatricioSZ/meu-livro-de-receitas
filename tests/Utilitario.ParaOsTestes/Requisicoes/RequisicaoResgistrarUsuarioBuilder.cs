using Bogus;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace Utilitario.ParaOsTestes.Requisicoes;

public class RequisicaoRegistrarUsuarioBuilder
{
    public static RequisicaoRegistrarUsuarioJson Construir(int tamanhoSenha = 10)
    {
        return new Faker<RequisicaoRegistrarUsuarioJson>()
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Senha, f => f.Internet.Password(tamanhoSenha))
            .RuleFor(
                requisicao => requisicao.Telefone, f => f.Phone.PhoneNumber(
                    "!#9########"
                ).Replace("!", $"{f.Random.Int(min: 1, max: 9)}")
            );
    }
}
