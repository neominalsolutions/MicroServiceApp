using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.Models;
using OrderService.Domain.OrderAggregate;
using OrderService.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace OrderService.Application.Features.Commands.Orders
{
  public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, string>
  {
    private readonly IOrderRepository orderRepository;
   



    public SubmitOrderCommandHandler(IOrderRepository orderRepository)
    {
      this.orderRepository = orderRepository;
    }

    public async Task<string> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {

      Order dbOrder = new(customerName: request.CustomerName, shipAddress: request.ShipAddress, cardTypeId: request.CardTypeId, cardNumber: request.CardNumber, cardSecurityNumber: request.CardSecurityNumber, cardHolderName: request.CardHolderName, cardExpiration: request.CardExpiration);

      foreach (var orderItem in request.BasketItems)
      {
        dbOrder.AddOrderItem(productName: orderItem.ProductName, productId: orderItem.ProductId, listPrice: orderItem.ListPrice, quantity: orderItem.Quantity);
      }

      await orderRepository.AddAsync(dbOrder);
      bool result =  await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

      if(!result)
      {
        throw new Exception("Sipariş oluşurken hata meydana geldi");
      }

      return dbOrder.Id;
    }
  }
}
