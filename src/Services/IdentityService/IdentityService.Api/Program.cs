using IdentityServer.Api.Application.Services;
using IdentityService.Api.Extensions.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IIdentityService, IdentityServer.Api.Application.Services.IdentityService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// config dosyasýndaki bilgilere göre
var config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile($"Configurations/appsettings.json", optional: false)
                    .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

// service olarak tanýt
builder.Services.AddConsul(config);





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}


app.UseAuthorization();

// yani þuanki uygulamamý uygulama ayaða kalktýðýnda register et diyoruz
app.UseConsul(app.Lifetime,config);

app.MapControllers();

app.Run();
