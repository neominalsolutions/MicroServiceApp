using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Domain.Models;
using OrderService.Domain.Models.CustomerAggregate;

namespace OrderService.Domain.Events
{
    public class PaymentMethodVerifiedDomainEvent : INotification
    {
        public Customer Customer{ get; private set; }
        public PaymentMethod Payment { get; private set; }
        public Order Order { get; private set; }

        public PaymentMethodVerifiedDomainEvent(Customer customer, PaymentMethod payment, Order order)
        {
            Customer = customer;
            Payment = payment;
            Order = order;
        }
    }
}
