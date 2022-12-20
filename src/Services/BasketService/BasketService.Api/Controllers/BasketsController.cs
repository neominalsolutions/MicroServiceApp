using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BasketsController : ControllerBase
  {

    [HttpGet]
    public IActionResult GetBasket()
    {
      return Ok("basket");
    }

  }
}
