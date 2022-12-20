using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {

    [HttpGet("list")]
    public IActionResult GetProducts()
    {
      return Ok("all-products");
    }

  }
}
