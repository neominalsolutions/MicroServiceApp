using IdentityServer.Api.Application.Services;
using IdentityService.Api.Extensions.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IIdentityService, IdentityServer.Api.Application.Services.IdentityService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//var config = new ConfigurationBuilder()
//                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
//                    .AddJsonFile($"Configurations/appsettings.json", optional: false)
//                    .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
//                    .AddEnvironmentVariables()
//                    .Build();


//builder.Services.AddCors(opt => {
//  opt.AddDefaultPolicy(policy =>
//  {

//    policy.AllowAnyHeader();
//    policy.AllowAnyOrigin();
//    policy.AllowAnyMethod();
//  });
//});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}





//builder.Services.ConfigureConsul(builder.Configuration);


//app.UseCors();

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
