using AutoMapper;

namespace Basket.Application.Mappers;

public static class BasketMapper
{
    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var config = new MapperConfiguration(c =>
        {
            c.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            c.AddProfile<BasketMappingProfile>();
        });
        var mapper = config.CreateMapper();
        return mapper;
    });
    
    public static IMapper Mapper => Lazy.Value;
}

