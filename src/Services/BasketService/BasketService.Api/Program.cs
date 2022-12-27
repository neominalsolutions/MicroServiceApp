using BasketService.Api.Extensions;
using BasketService.Api.Extensions.Registration;
using BasketService.Api.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{

  var securityScheme = new OpenApiSecurityScheme()
  {
    Description = "Basket API. Example: \"Authorization: Bearer {token}\"",
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

builder.Services.AddCors(opt => {
  opt.AddDefaultPolicy(policy =>
  {

    policy.AllowAnyHeader();
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
  });
});

string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile($"Configurations/appsettings.json", optional: false)
                    .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

builder.Configuration.AddConfiguration(config);

// JWT ile Token Based Auth yapalým

builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.AddSingleton(sp => sp.ConfigureRedis(builder.Configuration));
builder.Services.ConfigureConsul(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors();
//app.UseHttpsRedirection();

// middleware aktif edelim
app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
