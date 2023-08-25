using AutoMapper;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Application.Servicos.Automapper;



public class AutoMapperConfiguracao : Profile {

    public AutoMapperConfiguracao() {

        CreateMap<RequisicaoRegistrarUsuarioJson, Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());
        // .ForMember(destino => destino.Password, config => config.MapFrom(requisicao => requisicao.Senha));

        // A origem dos dados é RequisicaoRegistrarUsuarioJson e o destino é Usuario
        // Ele transforma RequisicaoRegistrarUsuarioJson em Usuário
    }

}
