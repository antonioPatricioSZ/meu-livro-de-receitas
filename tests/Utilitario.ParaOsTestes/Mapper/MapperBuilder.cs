using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Automapper;

namespace Utilitario.ParaOsTestes.Mapper;



public class MapperBuilder
{
    public static IMapper Instancia()
    {

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfiguracao());
        });
        return mockMapper.CreateMapper();
    }
}
