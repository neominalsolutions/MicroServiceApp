using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Dtos;

namespace ProductService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetCategories()
    {

      var data = new List<CategoryDto>
      {
        new CategoryDto
        {
          Id = 1,
          Name = "Category1 1",

        },
        new CategoryDto
        {
          Id = 2,
          Name = "Category 2",
        }
      };

      return Ok(data);
    }
  }
}
