using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Dtos;

namespace ProductService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {

    [HttpGet("list")]
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
