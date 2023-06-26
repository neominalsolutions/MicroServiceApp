
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
    private readonly IConfiguration configuration;


    public IdentityService(IConfiguration configuration)
    {
      this.configuration = configuration;
    }
    public Task<LoginResponseDto> Login(LoginRequestDto requestModel)
    {
      // DB Process will be here. Check if user information is valid and get details

      var key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");

      // VERIFY SIGNATURE KEY Şifreleme Algoritması HS256, 

      var tokenHandler = new JwtSecurityTokenHandler(); // token oluşturucu sınıf

      // token içerisindeki payload bilgisi
      var identity = new ClaimsIdentity(new Claim[]
         {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, requestModel.UserName),
                    new Claim("Roles",requestModel.Roles),
                    new Claim("scope", string.Join(" ", requestModel.Scopes))
         });

      // claim kullanıcıya ait sistem tarafından taşınan özellik

      // token içerisindeki kimlik bilgileri ve token algoritması ve expire time kullanılarak token oluşturmak için hazır hale getirir.
      var descriptor = new SecurityTokenDescriptor
      {
        Subject = identity, // Subject sistemdeki kullanıcı
        Expires = DateTime.UtcNow.AddHours(1), // ne kadar sürelik bir tok
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        // 256 bitlik bir şifreleme algoritması kullan. Simetrik algoritmalar hem şifrelenir hemde şifre çözülür
      };
      var token = tokenHandler.CreateToken(descriptor); // token nesnesi oluşur
      var accessToken = tokenHandler.WriteToken(token); // token nesnesini yazıp access Token üret.

 
      LoginResponseDto response = new()
      {
        AccessToken = accessToken,
        TokenType = "bearer"
      };



      return Task.FromResult(response);

    }
  }
}
