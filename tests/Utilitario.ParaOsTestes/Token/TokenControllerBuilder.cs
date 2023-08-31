using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;


public class TokenControllerBuilder {

    public static TokenController Instancia() {
        return new TokenController(1000, "KWk4TWprQHdVZC5RY2hkUCw2Xml8dUlZKC0uPkEobDdRLm9+VWM1eENteiNXN1VaMV0hczJrfkJAVyolWnkk");
    }

    public static TokenController TokenExpirado()
    {
        return new TokenController(0.0166667, "KWk4TWprQHdVZC5RY2hkUCw2Xml8dUlZKC0uPkEobDdRLm9+VWM1eENteiNXN1VaMV0hczJrfkJAVyolWnkk");
    }

}
