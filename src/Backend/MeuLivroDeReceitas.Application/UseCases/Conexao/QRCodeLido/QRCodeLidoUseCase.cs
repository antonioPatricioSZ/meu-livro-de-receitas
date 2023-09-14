using AutoMapper;
using HashidsNet;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;

public class QRCodeLidoUseCase : IQRCodeLidoUseCase {

    private readonly IHashids _hashids;
    private readonly IConexaoReadOnlyRepositorio _repositorioConexao;
    private readonly ICodigoReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;

    public QRCodeLidoUseCase(
        ICodigoReadOnlyRepositorio repositorio,
        IUsuarioLogado usuarioLogado,
        IConexaoReadOnlyRepositorio repositorioConexao,
        IHashids hashids
    ){
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _repositorioConexao = repositorioConexao;
        _hashids = hashids;
    }


    public async Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string usuarioQueGerouQRCode)> Executar(string codigoConexao) {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var codigo = await _repositorio.RecuperarEntidadeCodigo(codigoConexao);

        await Validar(codigo, usuarioLogado);

        var usuarioParaSeConectar = new RespostaUsuarioConexaoJson {
            Nome = usuarioLogado.Nome
        };

        return (usuarioParaSeConectar, _hashids.EncodeLong(codigo.UsuarioId));
    }


    private async Task Validar(
        Domain.Entidades.Codigos codigo,
        Domain.Entidades.Usuario usuarioLogado
    ){
        if(codigo is null) {
            throw new Exception();
        }

        if(codigo.UsuarioId == usuarioLogado.Id) {
            throw new Exception();
        }

        var existeConexao = await _repositorioConexao.ExisteConexao(
            codigo.UsuarioId, usuarioLogado.Id
        );

        if(existeConexao) {
            throw new Exception();
        }
    }

}
