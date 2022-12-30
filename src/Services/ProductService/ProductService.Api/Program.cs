using Microsoft.EntityFrameworkCore;
using ProductService.Api.Extensions.Registration;
using ProductService.Api.Infrastructure;
using ProductService.Api.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();











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

app.UseConsol(app.Lifetime, builder.Configuration);

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
