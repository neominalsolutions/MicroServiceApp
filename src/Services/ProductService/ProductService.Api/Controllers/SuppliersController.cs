using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Dtos;

namespace ProductService.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SuppliersController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetSuppliers()
    {

      var data = new List<SupplierDto>
      {
        new SupplierDto
        {
          Id = 1,
          Name = "Supplier 1",
  
        },
        new SupplierDto
        {
          Id = 2,
          Name = "Supplier 2",
        }
      };

      return Ok(data);
    }
  }
}
