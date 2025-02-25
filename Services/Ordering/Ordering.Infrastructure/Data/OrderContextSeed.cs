using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(GetOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation($"Ordering Database : {typeof(OrderContext).Name} seeded!");
        }
    }

    private static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
        {
            new()
            {
                UserName = "cagri",
                FirstName = "Çağrı",
                LastName = "Çağman",
                TotalPrice = 1000,
                EmailAddress = "cagri@gmail.com",
                AddressLine = "İstanbul",
                Country = "Turkey",
                State = "TR",
                ZipCode = "98052",
                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "cagri",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "cagri",
                LastModifiedDate = new DateTime()
            }
        };
    }
}