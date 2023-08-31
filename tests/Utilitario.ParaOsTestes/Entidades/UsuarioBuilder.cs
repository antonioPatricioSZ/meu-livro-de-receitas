﻿using Bogus;
using MeuLivroDeReceitas.Domain.Entidades;
using Utilitario.ParaOsTestes.Criptografia;

namespace Utilitario.ParaOsTestes.Entidades;


public class UsuarioBuilder {

    public static (Usuario usuario, string senha) Construir() {

        string senha  = string.Empty;

        var usuarioGerado = new Faker<Usuario>()
            .RuleFor(c => c.Id, _ => 1)
            .RuleFor(c => c.Nome, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Senha, f => {
                senha = f.Internet.Password();

                return EncriptadorDeSenhaBuilder.Instancia().Criptografar(senha);
            })
            .RuleFor(
                requisicao => requisicao.Telefone, f => f.Phone.PhoneNumber(
                    "!#9########"
                ).Replace("!", $"{f.Random.Int(min: 1, max: 9)}")
            );

        return (usuarioGerado, senha);
    }

} 
