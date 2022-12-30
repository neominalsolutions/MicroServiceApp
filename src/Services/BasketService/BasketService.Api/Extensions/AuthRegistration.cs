using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace BasketService.Api.Extensions
{
    public static class AuthRegistration
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
        {


      var key = Encoding.ASCII.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"); // aynı key

      // Basket Projesine bir authentication kimlik doğrulama yöntemi kazandırıyoruz.
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
        // JWT Authentication
                  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                  {
                    x.RequireHttpsMetadata = false; // http çalışıyoruz
                    x.SaveToken = true; // sistemin cacheinde sessionda bu token bilgisini sakla
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                      ValidateIssuerSigningKey = true, // SignKey alogoritmasına göre validate et.
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateIssuer = false, // Token Oluşturucu
                      ValidateAudience = false // Token istek yapan arkadaşları token validation için önemsemedik
                    };

                  });

      return services;
        }
    }
}
