
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace IdentityService.Api.Extensions.Registration
{
    public static class ConsulRegistration
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
      // consul servisini ConsulConfig:Address yere göre sisteme tanıtır.
      // uygyulama genelnde tek bir consul instance yeterli
      services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfig:Address"]; 
                consulConfig.Address = new Uri(address);
            }));

            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
      // identityservice,orderservice,basketservice

      var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();

      var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

      var uri = configuration.GetValue<Uri>("ConsulConfig:ServiceAddress");
      var serviceName = configuration.GetValue<string>("ConsulConfig:ServiceName");
      var serviceId = configuration.GetValue<string>("ConsulConfig:ServiceId");

      var registration = new AgentServiceRegistration()
      {
        ID = serviceId,
        Name = serviceName,
        Address = $"{uri.Host}",
        Port = uri.Port,
        Tags = new[] { serviceName, serviceId }
      };

      logger.LogInformation("Registering with Consul");
      //öncesinde consulClient.Agent servis silinir
      consulClient.Agent.ServiceDeregister(registration.ID).Wait();
      //daha sonra kendini register eder.
           consulClient.Agent.ServiceRegister(registration).Wait();

      // uygulama ilk ayağa kalktığı anda bu işlemi yapar.
      lifetime.ApplicationStopping.Register(() =>
      {
        logger.LogInformation("Deregistering from Consul");
        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
      });

      return app;
        }
    }
}
