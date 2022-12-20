using OrderService.Application.Repositories;
using OrderService.Domain.Models.CustomerAggregate;
using OrderService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
  public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
  {
    private readonly OrderContext dbContext;
    public CustomerRepository(OrderContext dbContext) : base(dbContext)
    {
      this.dbContext = dbContext;
    }
  }


}
