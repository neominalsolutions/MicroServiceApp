﻿using OrderService.Domain.Exceptions;
using OrderService.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models.CustomerAggregate
{
    public class PaymentMethod : AggregateRoot
    {
        public string Alias { get; set; }
        public string CardNumber { get; set; }
        public string SecurityNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime Expiration { get; set; }

        public int CardTypeId { get; set; }


        public CardType CardType { get; private set; }


        public PaymentMethod() { }

        public PaymentMethod(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration)
        {
            Id = Guid.NewGuid().ToString();  
            CardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingDomainException(nameof(cardNumber));
            SecurityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));
            CardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));


            Alias = alias;
            Expiration = expiration;
            CardTypeId = cardTypeId;
        }

        public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
        {
            return CardTypeId == cardTypeId
                && CardNumber == cardNumber
                && Expiration == expiration;
        }
    }
}
