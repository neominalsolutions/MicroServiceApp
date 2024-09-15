using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Api.Extensions.Registration;
using ProductService.Api.Infrastructure;
using ProductService.Api.IntegrationEvents;

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


builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization(opt =>
{
  opt.AddPolicy("read.user.policy", policy =>
  {
    policy.RequireAuthenticatedUser();
    policy.RequireClaim("scope", "read.user");
    policy.RequireRole("admin");
  });
});



builder.Services.ConfigureConsul(builder.Configuration);

  builder.Services.AddDbContext<ProductContext>(opt =>
  {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ProductDbConnectionString"));
    opt.EnableSensitiveDataLogging();
  });


builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();

builder.Services.AddCap(options =>
{
  options.UseEntityFramework<ProductContext>();
  options.UseSqlServer(builder.Configuration.GetConnectionString("ProductDbConnectionString"));

  options.UseRabbitMQ(options =>
  {
    options.ConnectionFactoryOptions = options =>
    {
      options.Ssl.Enabled = false;
      options.HostName = "localhost";
      options.UserName = "guest";
      options.Password = "guest";
      options.Port = 5672;
    };
  });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseConsol(app.Lifetime, builder.Configuration);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
