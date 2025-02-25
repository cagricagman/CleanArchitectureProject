using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure.Extensions;

public static class InfraServices
{
    public static IServiceCollection AddInfraService(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<OrderContext>(x=>x.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
        serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        return serviceCollection;
    }
}