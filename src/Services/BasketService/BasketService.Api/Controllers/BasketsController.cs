using BasketService.Api.Dtos;
using BasketService.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

  public class BasketsController : ControllerBase
  {

    private readonly IConsulHttpClient client;

    public BasketsController(IConsulHttpClient consulHttpClient)
    {
      client = consulHttpClient;
    }


    [HttpGet("items")]
    public async Task<IActionResult> GetBasket()
    {

      // Product Service Product Listesini alalım
      var data = await client.GetAsync<List<ProductDto>>("ProductService", "api/products/list");

      return Ok(data);
    }


  }
}
