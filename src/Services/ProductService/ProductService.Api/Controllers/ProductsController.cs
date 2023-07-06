using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Dtos;

namespace ProductService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize]
  public class ProductsController : ControllerBase
  {

    [HttpGet]
    [Authorize(Policy = "read.user.policy")]
    public IActionResult GetProducts()
   {

      var data = new List<ProductDto>
      {
        new ProductDto
        {
          ProductId = Guid.NewGuid().ToString(),
          ProductName = "Product 1",
          Price = 100
        },
        new ProductDto
        {
          ProductId = Guid.NewGuid().ToString(),
          ProductName = "Product 2",
          Price = 120
        }
      };

      return Ok(data);
    }

 

  }
}
