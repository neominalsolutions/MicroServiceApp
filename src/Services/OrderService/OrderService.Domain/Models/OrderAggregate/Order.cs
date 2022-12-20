using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.Models
{
  public class Order: BaseEntity
  {
    public string CustomerId { get; set; }
    public DateTime OrderDate { get; private set; }
    public string PaymentMethodId { get; set; }
    public string ShipAddress { get; set; }
    public int OrderStatusId { get; set; }


    private List<OrderItem> _orderItems = new List<OrderItem>();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order()
    {

    }

    public Order(string customerName, string shipAddress,  int cardTypeId, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration)
    {
      Id = Guid.NewGuid().ToString();
      OrderStatusId = OrderStatus.Submitted.Id;
      OrderDate = DateTime.UtcNow;
      ShipAddress = shipAddress;

      AddOrderSubmittedDomainEvent(customerName, cardTypeId, cardNumber,
                                 cardSecurityNumber, cardHolderName, cardExpiration);
    }


    private void AddOrderSubmittedDomainEvent(string customerName, int cardTypeId, string cardNumber,
               string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
    {
      var orderSubmittedDomainEvent = new OrderSubmittedDomainEvent(this, customerName, cardTypeId,
                                                                cardNumber, cardSecurityNumber,
                                                                cardHolderName, cardExpiration);

      this.AddDomainEvent(orderSubmittedDomainEvent);
    }

    public void AddOrderItem(string productName,string productId, decimal listPrice, int quantity)
    {
      var orderItem = new OrderItem(Id,productName,productId,quantity,listPrice);

      _orderItems.Add(orderItem);
    }

    public void SetCustomerId(string customerId)
    {
      CustomerId = customerId;
    }

    public void SetPaymentMethodId(string paymentMethodId)
    {
      PaymentMethodId = paymentMethodId;
    }

  }
}
