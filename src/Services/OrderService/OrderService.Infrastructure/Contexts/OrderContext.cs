

using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Models;
using OrderService.Domain.Models.CustomerAggregate;
using OrderService.Domain.SeedWork;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Contexts
{
  public class OrderContext:DbContext, IUnitOfWork
  {
    private readonly IMediator mediator;
    public OrderContext(DbContextOptions<OrderContext> dbContextOptions, IMediator mediator) :base(dbContextOptions)
    {
      this.mediator = mediator;
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<PaymentMethod> Payments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CardType> CardTypes { get; set; }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
      await mediator.DispatchDomainEventsAsync(this);
      await base.SaveChangesAsync(cancellationToken);

      return true;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

      // composite key
      modelBuilder.Entity<OrderItem>().HasKey(x => new { x.OrderId, x.ProductId });


      base.OnModelCreating(modelBuilder);
    }

  }
}
