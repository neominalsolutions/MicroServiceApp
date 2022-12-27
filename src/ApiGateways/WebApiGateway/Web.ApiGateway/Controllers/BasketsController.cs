using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.ApiGateway.Dtos;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Infrastructure;

namespace Web.ApiGateway.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BasketsController : ControllerBase
  {
    private readonly IConsulHttpClient httpClient;

    public BasketsController(IConsulHttpClient consulHttpClient)
    {
      httpClient = consulHttpClient;
    }


    [HttpPost("addBasket")]
    public async Task<IActionResult> AddBasketItem()
    {
      // Product Service Product Listesini alalım
      var data = await httpClient.GetAsync<List<ProductDto>>("ProductService", "api/products/list");


      return Ok();
    }
  }
}
