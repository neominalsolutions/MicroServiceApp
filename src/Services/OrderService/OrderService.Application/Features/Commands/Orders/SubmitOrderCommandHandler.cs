using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.IntegrationEvents;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.Models;
using OrderService.Domain.Models.CustomerAggregate;
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
    private readonly ICustomerRepository customerRepository;
    private readonly ICapPublisher bus; // service
    private readonly OrderContext orderContext;




    public SubmitOrderCommandHandler(IOrderRepository orderRepository, OrderContext orderContext, ICapPublisher bus)
    {
      this.orderRepository = orderRepository;
      this.orderContext = orderContext;
      this.bus = bus;
    }

    public async Task<string> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {

     

      using (var transaction = orderContext.Database.BeginTransaction(bus, autoCommit: false))
      {
        //your business logic code

        try
        {
          Order dbOrder = new(customerName: request.CustomerName, shipAddress: request.ShipAddress, cardTypeId: request.CardTypeId, cardNumber: request.CardNumber, cardSecurityNumber: request.CardSecurityNumber, cardHolderName: request.CardHolderName, cardExpiration: request.CardExpiration);

      

          foreach (var orderItem in request.BasketItems)
          {
            dbOrder.AddOrderItem(productName: orderItem.ProductName, productId: orderItem.ProductId, listPrice: orderItem.ListPrice, quantity: orderItem.Quantity);
          }

          await orderRepository.AddAsync(dbOrder);
          bool result = await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

          if (!result)
          {
            throw new Exception("Sipariş oluşurken hata meydana geldi");
          }


          var order = await this.orderRepository.GetById(dbOrder.Id);

          var PurchasedOrderItems = order.OrderItems.Select(a => new PurchasedOrderItem
          {
            Quantity = a.Quantity,
            ProductId = a.ProductId

          }).ToList();

          // Diğer MS'e Integration Event Fırlat

          var @event = new OrderCreatedIntegrationEvent(order.Id, PurchasedOrderItems);
          var headers = new Dictionary<string, string>();
          headers.Add("cap-corr-id", "aki");

          bus.Publish("OrderCreated", @event, headers);

          transaction.Commit();
        }
        catch (Exception)
        {
          transaction.Rollback();

          throw;
        }

        
      }

      return "1";
    }
  }
}
