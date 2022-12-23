using OrderService.Domain.Models;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.OrderAggregate
{
  public interface IOrderRepository: IRepository<Order>
  {
  }
}
