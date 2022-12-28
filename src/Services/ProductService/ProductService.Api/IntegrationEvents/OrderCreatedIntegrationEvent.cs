namespace ProductService.Api.IntegrationEvents
{
  public class PurchasedOrderItem
  {
    public int Quantity { get; set; }
    public string ProductId { get; set; }
  }
  public class OrderCreatedIntegrationEvent
  {
    public string OrderNumber { get; private set; }
    public List<PurchasedOrderItem> Items { get; private set; }

    public OrderCreatedIntegrationEvent(string orderNumber, List<PurchasedOrderItem> items)
    {
      OrderNumber = orderNumber;
      Items = items;

    }

  }
}
