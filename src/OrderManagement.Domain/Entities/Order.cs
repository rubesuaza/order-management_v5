using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Exceptions;
using OrderManagement.Domain.ValueObjects;

namespace OrderManagement.Domain.Entities;

/// <summary>
/// Aggregate root. Must contain at least one OrderItem and meet minimum 10.00 USD before transitioning to PAID.
/// </summary>
public class Order
{
    public Guid Id { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public Address? ShippingAddress { get; private set; }

    private Order() { }

    public static Order Create(IReadOnlyList<OrderItem> items)
    {
        if (items == null || items.Count == 0)
            throw new InvalidOrderStateException("Order must contain at least one item.");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Status = OrderStatus.Pending
        };
        foreach (var item in items)
            order._items.Add(item);
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
        var min = Money.Usd(10.00m);
        if (TotalAmount.Amount < 10.00m)
            throw new InvalidOrderStateException("Order must meet minimum value of 10.00 USD before transitioning to PAID.");
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
        if (Status != OrderStatus.Pending && Status != OrderStatus.Paid)
            throw new InvalidOrderStateException("CANCELLED is only possible from PENDING or PAID.");
        Status = OrderStatus.Cancelled;
    }

    public void SetShippingAddress(Address address) => ShippingAddress = address;
}
