using IdentityServer.Api.Application.Services;
using IdentityService.Api.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IIdentityService identityService;

        public AuthController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestModel)
        {
            var result = await identityService.Login(loginRequestModel);

         

            return Ok(result);
        }

    // api/auth/1 GET
    [HttpGet("{userId}")]
    public async Task<IActionResult> Login(string userId)
    {
   
      return Ok();
    }
  }
}
