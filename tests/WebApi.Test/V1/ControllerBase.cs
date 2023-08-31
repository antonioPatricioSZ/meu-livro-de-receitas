using System.Globalization;
using System.Net;
using System.Text;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Entidades;
using System.Text.Json;
using MeuLivroDeReceitas.Exceptions;
using Newtonsoft.Json;
using Xunit;

namespace WebApi.Test.V1;



public class ControllerBase : IClassFixture<MeuLivroReceitasWebApplicationFactory<Program>> {

    private readonly HttpClient _httpClient;

    public ControllerBase(MeuLivroReceitasWebApplicationFactory<Program> factory) {
        _httpClient = factory.CreateClient();
        ResourceMensagensDeErro.Culture = CultureInfo.CurrentCulture;
    }


    protected async Task<HttpResponseMessage> PostRequest(string metodo, object body) {
        var jsonString = JsonConvert.SerializeObject(body);
        return await _httpClient.PostAsync(metodo, new StringContent(
            jsonString, Encoding.UTF8, "application/json")
        );
    }


    protected async Task<HttpResponseMessage> PutRequest(string metodo, object body, string token = "") {
        // O token é opcional pq eu posso usar esse metodo para recuperar a senha do usuario entao 
        // eu não estarei logado qdo fizer isso
        AutorizarRequisicao(token);
        var jsonString = JsonConvert.SerializeObject(body);
        return await _httpClient.PutAsync(metodo, new StringContent(
            jsonString, Encoding.UTF8, "application/json")
        );
    }


    protected async Task<string> Login(string email, string senha) {

        var requisicao = new RequisicaoLoginJson {
            Email = email,
            Senha = senha
        };

        var respota = await PostRequest("login", requisicao);

        await using var respostaBody = await respota.Content.ReadAsStreamAsync();

        var respostaData = await JsonDocument.ParseAsync(respostaBody);

        return respostaData.RootElement.GetProperty("token").GetString();
    }


    private void AutorizarRequisicao(string token) {
        if(!string.IsNullOrEmpty(token)) {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
    }

}
