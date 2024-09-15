


using OrderService.Api.Extensions.Registration.Consul;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var config = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile($"Configurations/appsettings.json", optional: false)
                    .AddJsonFile($"Configurations/appsettings.{env}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

builder.Configuration.AddConfiguration(config);

builder.Services.AddPersistenceRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration(typeof(Program));
builder.Services.ConfigureConsul(builder.Configuration);


builder.Services.AddCap(options =>
{
  options.UseEntityFramework<OrderContext>();
  options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDbConnectionString"));

  options.UseDashboard(o => o.PathMatch = "/cap-dashboard");

  options.UseRabbitMQ(options =>
  {
    options.ConnectionFactoryOptions = options =>
    {
      options.Ssl.Enabled = false;
      options.HostName = "localhost"; // local haberleþme
      //options.HostName = "rabbitmq"; // dockerdan haberleþmek için, container name üzerinden haberleþelim.
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

//app.UseHttpsRedirection();

app.UseConsul(app.Lifetime, builder.Configuration);



app.UseAuthorization();

app.MapControllers();

app.Run();
