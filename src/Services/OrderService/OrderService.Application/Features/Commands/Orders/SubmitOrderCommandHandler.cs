using MediatR;
using OrderService.Application.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Commands.Orders
{
    public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, bool>
    {
        private readonly IOrderRepository orderRepository;
    //private readonly IEventBus eventBus;
    //private readonly ILogger<CreateOrderCommandHandler> logger;
    //IEventBus eventBus


        public SubmitOrderCommandHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
            //this.eventBus = eventBus;
            //this.logger = logger;
        }

        public async Task<bool> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
        {
          //logger.LogInformation("CreateOrderCommandHandler -> Handle method invoked");


            Order dbOrder = new(customerName:request.CustomerName,shipAddress:request.ShipAddress,cardTypeId:request.CardTypeId,cardNumber:request.CardNumber,cardSecurityNumber:request.CardSecurityNumber,cardHolderName:request.CardHolderName,cardExpiration:request.CardExpiration);

            foreach (var orderItem in request.BasketItems)
            {
              dbOrder.AddOrderItem(productName: orderItem.ProductName, productId: orderItem.ProductId, listPrice: orderItem.ListPrice, quantity: orderItem.Quantity);
            }

            await orderRepository.AddAsync(dbOrder);
            await orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            // sipariş alındığı anda eğer customer sistemde yoksa yeni bir customer eklenir.
            // customer sistemde varsa customerId alanı order üzerinden güncellenir.
            // customer payment method geçerli ise valid tru alanları o zaman sipariş oluşturulur aksi taktirde hata fırlatılır.

            // Bundan sonraki süreçte sipariş sonrası ürün stock güncellemesi için Integration Event Fırlatılır.

            //logger.LogInformation("CreateOrderCommandHandler -> dbOrder saved");

            //var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(request.UserName, dbOrder.Id);

            //eventBus.Publish(orderStartedIntegrationEvent);

            //logger.LogInformation("CreateOrderCommandHandler -> OrderStartedIntegrationEvent fired");

            return true;
        }
    }
}
