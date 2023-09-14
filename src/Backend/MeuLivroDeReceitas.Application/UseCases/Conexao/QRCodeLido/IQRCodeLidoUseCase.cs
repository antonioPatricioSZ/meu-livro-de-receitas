using MeuLivroDeReceitas.Comunicacao.Respostas;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;


public interface IQRCodeLidoUseCase {

    Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string usuarioQueGerouQRCode)> Executar(string codigoConexao);

}
