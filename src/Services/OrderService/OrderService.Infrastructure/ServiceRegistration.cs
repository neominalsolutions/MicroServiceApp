using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Models.CustomerAggregate;
using OrderService.Domain.OrderAggregate;
using OrderService.Infrastructure.Contexts;
using OrderService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(opt =>
            {
                opt.UseSqlServer(configuration["OrderDbConnectionString"]);
                opt.EnableSensitiveDataLogging();
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>()
                .UseSqlServer(configuration["OrderDbConnectionString"]);


            return services;
        }
    }
}
