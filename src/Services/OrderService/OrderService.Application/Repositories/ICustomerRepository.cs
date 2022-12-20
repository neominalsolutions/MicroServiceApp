using OrderService.Domain.Models;
using OrderService.Domain.Models.CustomerAggregate;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Repositories
{
  public interface ICustomerRepository:IGenericRepository<Customer>
  {
  }
}
