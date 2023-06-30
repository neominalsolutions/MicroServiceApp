using CacheManager.Core;
using Consul;
using Microsoft.OpenApi.Models;
using MMLib.Ocelot.Provider.AppConfiguration;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
  options.Folder = "Configurations";
});



// api gateway uygulamasýna jwt authentication dahil ettik.
// ayný secret key ile çalýþýyorlar

builder.Services.ConfigureAuth(builder.Configuration);
//builder.Services
//  .AddOcelot(builder.Configuration)
//  .AddCacheManager(x =>
//{
//  x.WithRedisConfiguration("redis",
//          config =>
//          {
//            config.WithAllowAdmin();
//            config.WithDatabase(0);
//            config.WithEndpoint("localhost", 6379);
//          })
//  .WithJsonSerializer()
//  .WithRedisCacheHandle("redis");
//}).AddConsul();


builder.Services
  .AddOcelot(builder.Configuration)
  .AddConsul();

builder.Services.AddSwaggerForOcelot(builder.Configuration);
// ocelotan sonra consul diye bir service ekliyoruz.






var app = builder.Build();


if (app.Environment.IsDevelopment())
{
  app.UseSwagger();

}

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerForOcelotUI(opt =>
  {
    opt.PathToSwaggerGenerator = "/swagger/docs";
  }).UseOcelot().Wait();

app.UseHttpsRedirection();

app.MapControllers();

//// ocelot middleware
//app.UseOcelot().Wait();

app.Run();
