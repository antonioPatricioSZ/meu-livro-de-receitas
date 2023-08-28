using System.Globalization;
using System.Text;
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

}
