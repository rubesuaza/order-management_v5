using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

/// <summary>
/// Aggregate root. Must contain at least one OrderItem and meet minimum 10.00 USD before transitioning to PAID.
/// </summary>
public class Order
{
    private const decimal MinimumOrderAmountForPayment = 10.00m;

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public Address? ShippingAddress { get; private set; }

    private Order() { }

    public static Order Create(IReadOnlyList<OrderItem> items) => Create(Guid.Empty, items);

    public static Order Create(Guid customerId, IReadOnlyList<OrderItem> items)
    {
        if (items == null || items.Count == 0)
            throw new InvalidOrderStateException("Order must contain at least one item.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = OrderStatus.Pending
        };
        order._items.AddRange(items);
        return order;
    }

    /// <summary>
    /// Reconstitutes an order from persistence. Used by infrastructure mappers.
    /// </summary>
    public static Order Reconstitute(Guid id, Guid customerId, OrderStatus status, IReadOnlyList<OrderItem> items)
    {
        var order = new Order
        {
            Id = id,
            CustomerId = customerId,
            Status = status
        };
        order._items.AddRange(items);
        return order;
    }

    public Money TotalAmount
    {
        get
        {
            if (_items.Count == 0) return Money.Usd(0);
            return _items.Aggregate(Money.Usd(0), (acc, i) => acc.Add(i.LineTotal));
        }
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOrderStateException($"Cannot mark as paid from status {Status}.");
        if (TotalAmount.Amount < MinimumOrderAmountForPayment)
            throw new InvalidOrderStateException($"Order must meet minimum value of {MinimumOrderAmountForPayment} USD before transitioning to PAID.");
        Status = OrderStatus.Paid;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOrderStateException("SHIPPED is only possible from PAID.");
        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (!IsInCancellableState())
            throw new InvalidOrderStateException("CANCELLED is only possible from PENDING or PAID.");
        Status = OrderStatus.Cancelled;
    }

    private bool IsInCancellableState() => Status == OrderStatus.Pending || Status == OrderStatus.Paid;

    public void SetShippingAddress(Address address) => ShippingAddress = address;
}
