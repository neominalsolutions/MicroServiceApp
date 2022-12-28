
using Microsoft.EntityFrameworkCore;
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

    public override async Task<Order> GetById(string id)
    {
      return await dbContext.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
    }

  }
}
