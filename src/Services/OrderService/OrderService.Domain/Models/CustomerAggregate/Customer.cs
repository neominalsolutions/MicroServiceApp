
using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.Models.CustomerAggregate
{
  public class Customer: BaseEntity, IAggregateRoot
  {
    public string CustomerName { get; set; }

    private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>();
    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    public Customer()
    {

    }
    public Customer(string customerName)
    {
      Id = Guid.NewGuid().ToString();
      CustomerName = customerName;

    }

    public PaymentMethod VerifyOrAddPaymentMethod(
           int cardTypeId, string alias, string cardNumber,
           string securityNumber, string cardHolderName, DateTime expiration, Order order)
    {
      var existingPayment = _paymentMethods.FirstOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

      if (existingPayment != null)
      {
        // raise event 
        AddDomainEvent(new PaymentMethodVerifiedDomainEvent(this, existingPayment, order));

        return existingPayment;
      }

      var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);

      _paymentMethods.Add(payment);

      // raise event 
      AddDomainEvent(new PaymentMethodVerifiedDomainEvent(this, payment, order));

      return payment;
    }

  }
}
