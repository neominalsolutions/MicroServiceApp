
using MediatR;
using OrderService.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Commands.Orders
{
    public class SubmitOrderCommand : IRequest<string>
    {

       public List<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();

        public string CustomerName { get; set; }

        public string ShipAddress { get; set; }

        public string CardNumber { get; set; }

        public string CardHolderName { get; set; }

        public DateTime CardExpiration { get; set; }

        public string CardSecurityNumber { get; set; }

        public int CardTypeId { get; set; } // master visa Amex


  }

   
}
