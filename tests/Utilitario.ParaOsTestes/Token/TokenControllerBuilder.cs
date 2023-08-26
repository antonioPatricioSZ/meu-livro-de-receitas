using MeuLivroDeReceitas.Application.Servicos.Criptografia;
using MeuLivroDeReceitas.Application.Servicos.Token;

namespace Utilitario.ParaOsTestes.Token;


public class TokenControllerBuilder {

    public static TokenController Instancia() {
        return new TokenController(1000, "MEMkR3ZMJVM4RDF5XnNjWDZiMTZQSVcqa2skd3JmdU85Yk0hMiR1eV5CM1dTZ0pMdWY=");
    }

}
