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
  //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class OrdersController : ControllerBase
  {

    private readonly IMediator mediator;

   
    private readonly OrderContext orderContext;
    private readonly IOrderRepository orderRepository;


    public OrdersController(IMediator mediator, OrderContext orderContext, IOrderRepository orderRepository)
    {
      this.mediator = mediator;
      this.orderContext = orderContext;
      this.orderRepository = orderRepository;

    }

    [HttpPost("submitOrder")]
    public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderCommand command)
    {
      var orderId = await mediator.Send(command);

  

      return Ok("");
    }
  }
}
