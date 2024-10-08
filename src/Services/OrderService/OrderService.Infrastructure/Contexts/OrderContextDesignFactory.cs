﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using OrderService.Infrastructure.Contexts;

namespace OrderService.Infrastructure.Context
{
    public class OrderContextDesignFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContextDesignFactory()
        {
        }

        public OrderContext CreateDbContext(string[] args)
        {
           // migration alırken kullanılacak.
            var connStr = "Data Source=localhost;Initial Catalog=OrderDb;Persist Security Info=True;User ID=sa;Password=Password1;TrustServerCertificate=True;";
            //var connStr = "Data Source=c_sqlserver;Initial Catalog=OrderDB;Trusted_Connection=True";

          var optionsBuilder = new DbContextOptionsBuilder<OrderContext>()
                .UseSqlServer(connStr);

            return new OrderContext(optionsBuilder.Options,new NoMediator());
        }

    class NoMediator : IMediator
    {
      public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
      {
        return default;
      }

      public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
      {
        return default;
      }

      public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
      {
        return Task.CompletedTask;
      }

      public Task Publish(object notification, CancellationToken cancellationToken = default)
      {
        return Task.CompletedTask;
      }

      public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
      {
        return Task.FromResult<TResponse>(default);
      }

      public Task<object> Send(object request, CancellationToken cancellationToken = default)
      {
        return Task.FromResult<object>(default);
      }
    }


  }
}
