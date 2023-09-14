//using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
//using MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
//using MeuLivroDeReceitas.Comunicacao.Respostas;
//using MeuLivroDeReceitas.Exceptions;
//using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.SignalR;

//namespace MeuLivroDeReceitas.Api.WebSockets;

//[Authorize(Policy = "UsuarioLogado")]
//public class AdicionarConexao : Hub
//{
//    uma classe estatica é compartilhada em todas as conexoes

//    private readonly Broadcaster _broadcaster;

//    private readonly IQRCodeLidoUseCase _qRCodeLidoUseCase;
//    private readonly IGerarQRCodeUseCase _gerarQRCodeUseCase;
//    private readonly IHubContext<AdicionarConexao> _hubContext;

//    public AdicionarConexao(
//        IGerarQRCodeUseCase gerarQRCodeUseCase,
//        IHubContext<AdicionarConexao> hubContext,
//        IQRCodeLidoUseCase qRCodeLidoUseCase
//    )
//    {
//        _broadcaster = Broadcaster.Instance;
//        _gerarQRCodeUseCase = gerarQRCodeUseCase;
//        _hubContext = hubContext;
//        _qRCodeLidoUseCase = qRCodeLidoUseCase;
//    }


//    public async Task GetQRCode()
//    {
//        (var qrCode, var idUsuario) = await _gerarQRCodeUseCase.Executar();

//        _broadcaster.InicializarConexao(_hubContext, idUsuario, Context.ConnectionId);

//        await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
//    }


//    public async Task QRCodeLido(string codigoConexao)
//    {
//        o filtro das exceptions não vai funcionar aqui

//        try
//        {
//            (var usuarioParaSeConectar, var usuarioQueGerouQRCode) = await _qRCodeLidoUseCase.Executar(codigoConexao);

//            var connectionId = _broadcaster.GetConnectionIdDoUsuario(usuarioQueGerouQRCode);

//            await Clients.Client(connectionId).SendAsync("ResultadoQRCodeLido", usuarioParaSeConectar);
//        }
//        catch (MeuLivroDeReceitasException ex)
//        {
//            await Clients.Caller.SendAsync("Erro", ex.Message);
//        }
//        catch
//        {
//            await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
//        }

//    }



//}
