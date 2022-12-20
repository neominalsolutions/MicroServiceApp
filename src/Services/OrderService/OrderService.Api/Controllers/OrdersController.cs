using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Commands.Orders;

namespace OrderService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {

    private readonly IMediator mediator;

    public OrdersController(IMediator mediator)
    {
      this.mediator = mediator;
    }

    [HttpPost("submitOrder")]
    public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderCommand command)
    {
      var res = await mediator.Send(command);

      return Ok(res);
    }
  }
}
