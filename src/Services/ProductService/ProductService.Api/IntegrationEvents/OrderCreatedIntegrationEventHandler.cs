using DotNetCore.CAP;

namespace ProductService.Api.IntegrationEvents
{
  public class OrderCreatedIntegrationEventHandler: ICapSubscribe
  {
    [CapSubscribe("OrderCreated")]
    public void Consumer(OrderCreatedIntegrationEvent @event)
    {

    }
  }
}
