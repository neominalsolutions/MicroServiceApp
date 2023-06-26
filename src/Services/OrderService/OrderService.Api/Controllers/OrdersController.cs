using DotNetCore.CAP;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.IntegrationEvents;
using OrderService.Application.Features.Commands.Orders;
using OrderService.Domain.OrderAggregate;
using OrderService.Infrastructure.Contexts;

namespace OrderService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class OrdersController : ControllerBase
  {

    private readonly IMediator mediator;

    private readonly ICapPublisher bus;
    private readonly OrderContext orderContext;
    private readonly IOrderRepository orderRepository;


    public OrdersController(IMediator mediator, ICapPublisher bus, OrderContext orderContext, IOrderRepository orderRepository)
    {
      this.mediator = mediator;
      this.orderContext = orderContext;
      this.orderRepository = orderRepository;
      this.bus = bus;
    }

    [HttpPost("submitOrder")]
    public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderCommand command)
    {
      var orderId = await mediator.Send(command);

      // 
      var order = await this.orderRepository.GetById(orderId);

      var PurchasedOrderItems = order.OrderItems.Select(a => new PurchasedOrderItem
      {
        Quantity = a.Quantity,
        ProductId = a.ProductId

      }).ToList();

      var @event = new OrderCreatedIntegrationEvent(orderId,PurchasedOrderItems);

      using (var transaction = orderContext.Database.BeginTransaction(bus,autoCommit:true))
      {
        //your business logic code

        bus.Publish("OrderCreated", @event);
      }

      return Ok("");
    }
  }
}
