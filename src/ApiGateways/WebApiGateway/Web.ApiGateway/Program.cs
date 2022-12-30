using Consul;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();


builder.Host.ConfigureAppConfiguration((host, config) =>
{
  config.SetBasePath(host.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("Configurations/ocelot.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
});






// api gateway uygulamasýna jwt authentication dahil ettik.
// ayný secret key ile çalýþýyorlar

builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.AddOcelot().AddConsul();
// ocelotan sonra consul diye bir service ekliyoruz.







var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ocelot middleware
app.UseOcelot().Wait();

app.Run();
