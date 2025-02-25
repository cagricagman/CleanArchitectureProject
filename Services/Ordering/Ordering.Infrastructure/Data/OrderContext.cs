using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var item in ChangeTracker.Entries<EntityBase>())
        {
            switch (item.State)
            {
                case EntityState.Added:
                    item.Entity.CreatedBy = "cagri"; //TODO Auth server devreye alınınca değişecek
                    item.Entity.CreatedDate = DateTime.Now;
                    break;
                case EntityState.Modified:
                    item.Entity.LastModifiedBy = "cagri";//TODO Auth server devreye alınınca değişecek
                    item.Entity.LastModifiedDate = DateTime.Now;
                    break;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}