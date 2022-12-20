using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOcelot().AddConsul();
builder.Services.ConfigureAuth(builder.Configuration);

builder.Host.ConfigureAppConfiguration((host, config) =>
{
  config.SetBasePath(host.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("Configurations/ocelot.json")
                        .AddEnvironmentVariables();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ocelot middleware
await app.UseOcelot();

app.Run();
