using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Dtos
{
  public class OrderItemDto
  {
    public string ProductId { get; init; }

    public string ProductName { get; init; }

    public decimal ListPrice { get; init; }

    public int Quantity { get; init; }

  }
}
