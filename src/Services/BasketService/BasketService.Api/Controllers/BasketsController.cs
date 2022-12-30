using BasketService.Api.Dtos;
using BasketService.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BasketService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  // Default JWT Scheme Bearer ismindedir.

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

      //var auth = await HttpContext.AuthenticateAsync();
      // access token bilgisini 
      var accessToken = await HttpContext.GetTokenAsync("access_token");
      // direkt olarak acesss Token bilgisi erişip token Decode işlemi yapabiliriz.

      var handler = new JwtSecurityTokenHandler();

      var jsonToken = handler.ReadToken(accessToken);

      //if (auth.Succeeded)
      //{

      //}

      // Product Service Product Listesini alalım
      var data = await client.GetAsync<List<ProductDto>>("ProductService", "api/products/list");

      return Ok(data);
    }


  }
}
