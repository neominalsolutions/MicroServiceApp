
using OrderService.Domain.Models;
using OrderService.Domain.OrderAggregate;
using OrderService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
  public class OrderRepository : GenericRepository<Order>, IOrderRepository
  {
    private readonly OrderContext dbContext;

    public OrderRepository(OrderContext dbContext) : base(dbContext)
    {
      this.dbContext = dbContext;
    }

  }
}
