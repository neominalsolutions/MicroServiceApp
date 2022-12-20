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
    public class UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler : INotificationHandler<PaymentMethodVerifiedDomainEvent>
    {
    

        public UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler()
        {
           
        }

        public async Task Handle(PaymentMethodVerifiedDomainEvent customerPaymentMethodVerifiedEvent, CancellationToken cancellationToken)
        {

            customerPaymentMethodVerifiedEvent.Order.SetCustomerId(customerPaymentMethodVerifiedEvent.Customer.Id);
            customerPaymentMethodVerifiedEvent.Order.SetPaymentMethodId(customerPaymentMethodVerifiedEvent.Payment.Id);

            // set methods so validate
        }
    }
}
