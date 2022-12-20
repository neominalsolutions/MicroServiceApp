using Microsoft.Extensions.DependencyInjection;


namespace OrderService.Api.Extensions.Registration
{
    public static class EventHandlerRegistration
    {
        public static IServiceCollection ConfigureEventHandlers(this IServiceCollection services)
        {
            //services.AddTransient<OrderCreatedIntegrationEventHandler>();

            return services;
        }
    }
}
