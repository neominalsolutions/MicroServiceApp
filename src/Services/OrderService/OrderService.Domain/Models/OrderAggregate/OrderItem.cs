using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Models
{
  public class OrderItem
  {
    public string OrderId { get; private set; }
    public string ProductName { get; private set; }
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal ListPrice { get; private set; }

    public OrderItem(string orderId,string productName, string productId, int quantity, decimal listPrice)
    {
      OrderId = orderId;
      ProductName = productName;
      ProductId = productId;
      Quantity = quantity;
      ListPrice = listPrice;
    }

  }
}
