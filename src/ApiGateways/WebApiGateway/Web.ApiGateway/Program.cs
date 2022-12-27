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


builder.Services.AddSwaggerGen(opt =>
{

  var securityScheme = new OpenApiSecurityScheme()
  {
    Description = "Web API Gateway. Example: \"Authorization: Bearer {token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT" // Optional
  };

  var securityRequirement = new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "bearerAuth"
            }
        },
        new string[] {}
    }
};

  opt.AddSecurityDefinition("bearerAuth", securityScheme);
  opt.AddSecurityRequirement(securityRequirement);
});


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



// consul client servisi tanýmladýk
builder.Services.AddSingleton<IConsulClient>(consul => new ConsulClient(consulConfig =>
{
  consulConfig.Address = new Uri("http://localhost:8500");
}));

builder.Services.AddTransient<IConsulHttpClient, ConsulHttpClient>();



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
