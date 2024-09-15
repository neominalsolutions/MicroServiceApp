using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Dtos;

namespace ProductService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class ProductsController : ControllerBase
  {

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "read.user.policy")]
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

    [HttpGet("{id}")]
    public IActionResult GetProductsById(int id)
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
