
using IdentityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Api.Application.Services
{
  public class IdentityService : IIdentityService
  {
    public Task<LoginResponseDto> Login(LoginRequestDto requestModel)
    {
      // DB Process will be here. Check if user information is valid and get details

      var key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");

      var tokenHandler = new JwtSecurityTokenHandler();
      var identity = new ClaimsIdentity(new Claim[]
         {
                    new Claim(ClaimTypes.Name, requestModel.UserName),
                    new Claim(ClaimTypes.Role, "Admin")
         });

      var descriptor = new SecurityTokenDescriptor
      {
        Subject = identity,
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(descriptor);
      var accessToken = tokenHandler.WriteToken(token);

 
      LoginResponseDto response = new()
      {
        AccessToken = accessToken,
        TokenType = "bearer"
      };



      return Task.FromResult(response);

    }
  }
}
