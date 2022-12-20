
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

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, requestModel.UserName),
                new Claim(ClaimTypes.Name, "Mert Alptekin"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TokenBasedAuthenticationKeyShouldBeLong"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseDto response = new()
            {
                AccessToken = encodedJwt,
                UserName = requestModel.UserName
            };

            return Task.FromResult(response);
        }
    }
}
