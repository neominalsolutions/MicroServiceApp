using MediatR;
using OrderService.Application.Repositories;
using OrderService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.DomainEventHandlers
{
    class OrderSubmittedDomainEventHandler : INotificationHandler<OrderSubmittedDomainEvent>
    {
        private readonly ICustomerRepository customerRepo;

        public OrderSubmittedDomainEventHandler(ICustomerRepository buyerRepository)
        {
            this.customerRepo = buyerRepository;
        }

        public async Task Handle(OrderSubmittedDomainEvent orderSubmittedEvent, CancellationToken cancellationToken)
        {
            var cardTypeId = (orderSubmittedEvent.CardTypeId != 0) ? orderSubmittedEvent.CardTypeId : 1;

            var customer = await customerRepo.GetSingleAsync(i => i.CustomerName == orderSubmittedEvent.CustomerName, i => i.PaymentMethods);

            bool buyerOriginallyExisted = customer != null;

            if (!buyerOriginallyExisted)
            {
                customer = new Domain.Models.CustomerAggregate.Customer(orderSubmittedEvent.CustomerName);
            }

        // burada  UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler tetikleniyor.
        // order'a paymentId ve CustomerId güncelleniyor ekleniyor.
        customer.VerifyOrAddPaymentMethod(cardTypeId,
                                           $"Payment Method on {DateTime.UtcNow}",
                                           orderSubmittedEvent.CardNumber,
                                           orderSubmittedEvent.CardSecurityNumber,
                                           orderSubmittedEvent.CardHolderName,
                                           orderSubmittedEvent.CardExpiration,
                                           orderSubmittedEvent.Order);

            var customerUpdated = buyerOriginallyExisted ?
                customerRepo.Update(customer) :
                await customerRepo.AddAsync(customer);

            await customerRepo.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            // order status changed event may be fired here
        }
    }
}
