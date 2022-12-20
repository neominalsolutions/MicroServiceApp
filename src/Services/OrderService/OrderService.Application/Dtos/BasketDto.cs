using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Dtos
{
  public class BasketDto
  {
    public List<BasketItemDto> Items => new List<BasketItemDto>();
    public decimal Total { get { return Items.Sum(x => x.Quantity * x.ListPrice); } }

  }
}
